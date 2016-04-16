using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Factories;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Additional;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;
using NLog;

namespace Dexpa.Core.Services
{
    public class OrderService : IOrderService
    {
        private const int DRIVER_PENALTY = 300;

        private IOrderRepository mRepository;

        private IDriverRepository mDriverRepository;

        private ICustomerRepository mCustomerRepository;

        private IEventRepository mEventRepository;

        private IOrderHistoryRepository mOrderHistoryRepository;

        private ITransactionService mTransactionService;

        private IRegionService mRegionService;

        private IGeocoderService mGeocodeService;

        private ITariffRepository mTariffRepository;

        private IGlobalSettingsRepository mGlobalSettingsRepository;

        private static Dictionary<OrderStateType, List<OrderStateType>> mAllowedOrderStates;

        private Logger mLogger = LogManager.GetCurrentClassLogger();

        private RegionBinaryStorage mRegionBinaryStorage;

        private IDriverWorkConditionsService mDriverWorkConditionsService;

        private IDriverOrderRequestRepository mDriverOrderRequestRepository;

        public OrderService(IOrderRepository repository, IDriverRepository driverRepository,
            ICustomerRepository customerRepository,
            IEventRepository eventRepository,
            IOrderHistoryRepository orderHistoryRepository,
            ITransactionService transactionService,
            IRegionService regionService,
            IGeocoderService geocoderService,
            ITariffRepository tariffRepository,
            IGlobalSettingsRepository globalSettingsRepository,
            RegionBinaryStorage regionBinaryStorage,
            IDriverWorkConditionsService driverWorkConditionsService,
            IDriverOrderRequestRepository driverOrderRequestRepository)
        {
            mRegionBinaryStorage = regionBinaryStorage;
            mRepository = repository;
            mDriverRepository = driverRepository;
            mCustomerRepository = customerRepository;
            mEventRepository = eventRepository;
            mOrderHistoryRepository = orderHistoryRepository;
            mTransactionService = transactionService;
            mRegionService = regionService;
            mGeocodeService = geocoderService;
            mTariffRepository = tariffRepository;
            mGlobalSettingsRepository = globalSettingsRepository;
            mDriverWorkConditionsService = driverWorkConditionsService;
            mDriverOrderRequestRepository = driverOrderRequestRepository;
        }

        static OrderService()
        {
            mAllowedOrderStates = new Dictionary<OrderStateType, List<OrderStateType>>();
            mAllowedOrderStates.Add(OrderStateType.Created, new List<OrderStateType>(new[] { OrderStateType.Assigned, OrderStateType.Canceled, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Canceled, new List<OrderStateType>());
            mAllowedOrderStates.Add(OrderStateType.Assigned, new List<OrderStateType>(new[] { OrderStateType.Canceled, OrderStateType.Accepted, OrderStateType.Rejected, OrderStateType.Created, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Rejected, new List<OrderStateType>(new[] { OrderStateType.Created, OrderStateType.Canceled }));
            mAllowedOrderStates.Add(OrderStateType.Accepted, new List<OrderStateType>(new[] { OrderStateType.Canceled, OrderStateType.Approved, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Failed, new List<OrderStateType>(new[] { OrderStateType.Canceled }));
            mAllowedOrderStates.Add(OrderStateType.Approved, new List<OrderStateType>(new[] { OrderStateType.Canceled, OrderStateType.Driving, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Driving, new List<OrderStateType>(new[] { OrderStateType.Canceled, OrderStateType.Waiting, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Waiting, new List<OrderStateType>(new[] { OrderStateType.Canceled, OrderStateType.Transporting, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Transporting, new List<OrderStateType>(new[] { OrderStateType.Completed, OrderStateType.Failed }));
            mAllowedOrderStates.Add(OrderStateType.Completed, new List<OrderStateType>());
        }

        public Order GetOrder(long orderId)
        {
            return mRepository.Single(o => o.Id == orderId);
        }

        public Order AddOrder(Order order, Customer customer = null)
        {
            UpdateOrderRelations(order);
            if (customer != null)
            {
                UpdateOrderCustomer(order, customer);
            }

            order = mRepository.Add(order);
            mRepository.Commit();

            AddAddOrderHistory(order, "");

            AddOrderStateChangedEvent(order, true);
            SetOrderMinCost(order);
            UpdateAddressLocation(order);
            mRepository.Commit();

            if (order.DriverId != null)
            {
                order.State = OrderStateType.Assigned;
                UpdateOrder(order, null);
            }

            return order;
        }

        private void UpdateAddressLocation(Order order)
        {
            if (order.Source != OrderSource.Yandex)
            {
                if (!string.IsNullOrEmpty(order.FromAddress.FullName))
                {
                    var fromPoint = mGeocodeService.Geocoding(order.FromAddress.FullName);
                    order.FromAddress.Latitude = fromPoint.Latitude;
                    order.FromAddress.Longitude = fromPoint.Longitude;
                }

                if (!string.IsNullOrEmpty(order.ToAddress.FullName))
                {
                    var toPoint = mGeocodeService.Geocoding(order.ToAddress.FullName);
                    order.ToAddress.Latitude = toPoint.Latitude;
                    order.ToAddress.Longitude = toPoint.Longitude;
                }
            }
        }

        public void DeleteOrder(long orderId)
        {
            var order = mRepository.Single(o => o.Id == orderId);
            if (order != null)
            {
                mRepository.Delete(order);
                mRepository.Commit();
            }
        }

        public IList<Order> GetOrders(DateTime date)
        {
            var fromDate = date.Date;
            var toDate = fromDate.AddDays(1);
            return GetOrders(fromDate, toDate);
        }

        public Order UpdateOrder(Order order, string updateReason, Customer customer = null)
        {
            UpdateOrderRelations(order);
            if (customer != null)
            {
                UpdateOrderCustomer(order, customer);
            }

            bool orderStateChanged = mRepository.IsItemPropertyChanged(order, "State");
            if (orderStateChanged)
            {
                var oldOrderState = (OrderStateType)mRepository.GetOldValue(order, "State");
                var newOrderState = order.State;
                if (!CheckOrderStateConsistency(oldOrderState, newOrderState))
                {
                    mRepository.ResetState(order);
                    var message = string.Format("Order state restricted by order consistency check. Current state {0}, new state {1}",
                        oldOrderState, newOrderState);
                    throw new OrderConsistencyException(message);
                }
                //Яндекс подтвердил заказ или водитель выехал к клиенту. Нужно разблокировать водителя (чтобы мог принимать заказы).
                if (order.State == OrderStateType.Canceled ||
                    order.State == OrderStateType.Approved ||
                    order.State == OrderStateType.Driving ||
                    order.State == OrderStateType.Failed ||
                    order.State == OrderStateType.Rejected)
                {
                    mRepository.UnlockDrivers(order.Id);
                }
            }

            if (mRepository.IsItemPropertyChanged(order, "DriverId"))
            {
                DriverReplaced(order);
            }
            //Это должно идти после DriverReplaced
            if (order.Driver != null && orderStateChanged)
            {
                mLogger.Debug("Order {1}({2}) state changed to {0}. Update driver state", order.State, order.Id, order.SourceOrderId);
                UpdateDriverState(order);
            }

            if (mRepository.IsItemPropertyChanged(order, "FromAddress") ||
                mRepository.IsItemPropertyChanged(order, "ToAddress"))
            {
                UpdateAddressLocation(order);
            }

            var updateOrder = mRepository.Update(order);

            AddOrderStateChangedEvent(order);

            if (order.State == OrderStateType.Canceled ||
                order.State == OrderStateType.Rejected ||
                order.State == OrderStateType.Failed)
            {
                AddCancelOrderHistory(order, updateReason);
            }
            else
            {
                AddUpdateOrderHistory(order, updateReason);
            }

            if (mRepository.IsItemPropertyChanged(order, "FromAddress") ||
                mRepository.IsItemPropertyChanged(order, "ToAddress"))
            {
                SetOrderMinCost(order);
            }
            mRepository.Commit();
            mDriverRepository.Commit();

            //If order state was changed to Accepted and it's not an yandex order, automatic approve it.
            if (order.Source != OrderSource.Yandex &&
                order.DriverId.HasValue &&
                (order.State == OrderStateType.Accepted ||
                order.State == OrderStateType.Assigned ||
                order.State == OrderStateType.Created))
            {
                order.State = OrderStateType.Approved;
                return UpdateOrder(order, "Автоматическое подтверждение заказа");
            }

            if (order.State == OrderStateType.Completed && order.Cost > 0)
            {
                if (order.Customer.Organization != null) //Безналичный расчет
                {
                    AddMoneyForOrder(order);
                }
                TakeDriverCommision(order);
            }

            if (orderStateChanged &&
                (order.State == OrderStateType.Failed || order.State == OrderStateType.Rejected))
            {
                if (order.DriverId != null)
                {
                    TakeDriverPenalty(order);
                }
            }

            UpdateDriverRequests(order);

            return updateOrder;
        }

        private void UpdateDriverRequests(Order order)
        {
            if (order.State == OrderStateType.Rejected && order.Driver != null)
            {
                var request = new DriverOrderRequest
                {
                    Driver = order.Driver,
                    DriverId = order.Driver.Id,
                    Order = order,
                    OrderId = order.Id,
                    State = OrderRequestState.Rejected
                };
                mDriverOrderRequestRepository.Add(request);
                mDriverOrderRequestRepository.Commit();
            }

            if (order.State == OrderStateType.Canceled ||
                order.State == OrderStateType.Failed ||
                order.State == OrderStateType.Completed)
            {
                var requests = mDriverOrderRequestRepository.List(r => r.OrderId == order.Id);
                foreach (var request in requests)
                {
                    mDriverOrderRequestRepository.Delete(request);
                }
                mDriverOrderRequestRepository.Commit();
            }
        }

        private void DriverReplaced(Order order)
        {
            var oldDriverIdObject = mRepository.GetOldValue(order, "DriverId");
            if (oldDriverIdObject == null)
            {
                return;
            }
            var oldDriverId = (long)oldDriverIdObject;
            var oldDriver = mDriverRepository.Single(d => d.Id == oldDriverId);
            if (oldDriver != null)
            {
                var newDriver = order.Driver;
                mLogger.Debug("Driver {0} replaced by {1} for order {2}",
                    newDriver != null ? newDriver.Id : -1,
                    oldDriver != null ? oldDriver.Id : -1,
                    order.Id);

                if (oldDriver != null)
                {
                    oldDriver.State = DriverState.ReadyToWork;
                    mDriverRepository.Update(oldDriver);
                }

                if (newDriver != null)
                {
                    newDriver.State = DriverState.Busy;
                    mDriverRepository.Update(newDriver);
                }
            }
        }

        private void TakeDriverPenalty(Order order)
        {
            mTransactionService.AddTransaction(new Transaction
            {
                Amount = DRIVER_PENALTY,
                Order = order,
                Driver = order.Driver,
                DriverId = order.Driver.Id,
                Comment = "Штраф за отмененный заказ #" + order.Id,
                PaymentMethod = PaymentMethod.NonCash,
                Group = TransactionGroup.Fine,
                Type = TransactionType.Withdrawal
            });
        }

        private void TakeDriverCommision(Order order)
        {
            try
            {
                var driver = order.Driver;
                mTransactionService.AddTransaction(new Transaction
                {
                    Amount = order.Cost * (GetComissionPercent(driver.WorkConditionsId.Value, order) / 100.0),
                    PaymentMethod = PaymentMethod.NonCash,
                    Driver = driver,
                    DriverId = order.DriverId.Value,
                    Group = TransactionGroup.OrderFee,
                    Type = TransactionType.Withdrawal,
                    Comment = "Комиссия за заказ #" + order.Id,
                    Order = order
                });
            }
            catch (Exception)
            {
                mLogger.Error("TakeDriverCommision. Driver {0} has not work conditions", order.DriverId);
            }

        }

        private double GetComissionPercent(long conditionId, Order order)
        {
            var condition = mDriverWorkConditionsService.GetWorkConditions(conditionId);
            var fees = condition.OrderFees;

            OrderFee orderFee = null;

            if (order.Source == OrderSource.Yandex)
            {
                orderFee = fees.FirstOrDefault(c => c.OrderType == OrderType.Yandex);
            }
            else if (order.Customer.OrganizationId != null)
            {
                orderFee = fees.FirstOrDefault(c => c.OrderType == OrderType.NonCash);
            }
            else if (order.IsAirport)
            {
                orderFee = fees.FirstOrDefault(c => c.OrderType == OrderType.Airport);
            }
            else
            {
                orderFee = fees.FirstOrDefault(c => c.OrderType == OrderType.None);
            }

            if (orderFee != null)
            {
                return orderFee.Value;
            }

            return 0.0;
        }

        private void AddMoneyForOrder(Order order)
        {
            mTransactionService.AddTransaction(new Transaction
            {
                Amount = order.Cost,
                PaymentMethod = PaymentMethod.NonCash,
                Driver = order.Driver,
                DriverId = order.DriverId.Value,
                Group = TransactionGroup.Other,
                Type = TransactionType.Replenishment,
                Comment = "Безналичный расчет по заказу #" + order.Id,
                Order = order
            });
        }

        private void UpdateDriverState(Order order)
        {
            if (order.State == OrderStateType.Approved && order.Source == OrderSource.Yandex ||
                order.State == OrderStateType.Driving)
            {
                order.Driver.State = DriverState.Busy;
                mDriverRepository.Update(order.Driver);
                mLogger.Debug("Driver state {0} changed to {1}", order.Driver.Id, order.Driver.State);
            }
            else if (order.State == OrderStateType.Completed ||
                     order.State == OrderStateType.Canceled ||
                     order.State == OrderStateType.Rejected ||
                     order.State == OrderStateType.Failed)
            {
                order.Driver.State = DriverState.ReadyToWork;
                mDriverRepository.Update(order.Driver);
                mLogger.Debug("Driver state {0} changed to {1}", order.Driver.Id, order.Driver.State);
            }

        }

        private void UpdateOrderCustomer(Order order, Customer customer)
        {
            if (customer.Id <= 0)
            {
                var existsCustomer = mCustomerRepository.Single(c => c.Phone == customer.Phone);
                if (existsCustomer != null)
                {
                    existsCustomer.Name = customer.Name;
                    customer = mCustomerRepository.Update(existsCustomer);
                }
                else
                {
                    customer = CustomerFactory.CreateCustomer(customer.Name, customer.Phone);
                    customer = mCustomerRepository.Add(customer);
                }
            }
            else
            {
                var existsCustomer = mCustomerRepository.Single(c => c.Id == customer.Id);
                existsCustomer.Name = customer.Name;
                customer = mCustomerRepository.Update(existsCustomer);
            }
            order.CustomerId = customer.Id;
        }

        private bool CheckOrderStateConsistency(OrderStateType oldOrderState, OrderStateType newOrderState)
        {
            return true;
            return mAllowedOrderStates[oldOrderState].Contains(newOrderState);
        }

        public IList<Order> GetOrders(DateTime fromDate, DateTime toDate, long? driverId, OrderStateType? orderStateType = null)
        {
            var orders = mRepository
                .List(o => o.DepartureDate >= fromDate &&
                           o.DepartureDate <= toDate &&
                           o.Driver.Id == driverId &&
                           (!orderStateType.HasValue || o.State == orderStateType));
            return orders;
        }

        public IList<Order> GetOrders(DateTime fromDate, DateTime toDate, OrderStateType? orderStateType = null)
        {
            var orders = mRepository
                .List(o => o.DepartureDate >= fromDate &&
                           o.DepartureDate <= toDate &&
                           (!orderStateType.HasValue || o.State == orderStateType));
            return orders;
        }

        public IList<Order> GetUnassignedOrders()
        {
            var fromDate = DateTime.UtcNow.Date.AddDays(-10);
            var toDate = DateTime.UtcNow.Date.AddDays(10);
            var orders = mRepository
                .List(o => o.DepartureDate >= fromDate &&
                           o.DepartureDate <= toDate &&
                           (o.DriverId == null) &&
                           o.State == OrderStateType.Created);
            return orders;
        }

        public int GetTotalOrdersCount()
        {
            return mRepository.Count();
        }

        private void UpdateOrderRelations(Order order)
        {
            Customer customer = null;
            Driver driver = null;
            Tariff tariff = null;

            if (order.CustomerId == 0)
            {
                order.CustomerId = null;
            }
            if (order.DriverId == 0)
            {
                order.DriverId = null;
            }
            if (order.TariffId == 0)
            {
                order.TariffId = null;
            }

            if (order.CustomerId.HasValue)
            {
                customer = mCustomerRepository.Single(c => c.Id == order.CustomerId);
            }

            if (order.DriverId.HasValue)
            {
                driver = mDriverRepository.Single(d => d.Id == order.DriverId);
            }

            if (order.TariffId.HasValue)
            {
                tariff = mTariffRepository.Single(t => t.Id == order.TariffId);
            }

            order.Tariff = tariff;
            order.Customer = customer;
            order.Driver = driver;
        }

        public IList<Order> GetOrders()
        {
            var date = DateTime.UtcNow;
            var orders = mRepository.List(o => o.DepartureDate >= date).OrderByDescending(o => o.DepartureDate).OrderBy(o => o.State).ToList();
            return orders;
        }

        public IList<OrderWithPriority> GetActiveOrders()
        {
            var date = DateTime.UtcNow.AddHours(-6);
            var date2 = DateTime.UtcNow.AddHours(6);
            var orders = mRepository.List(o => o.DepartureDate >= date && o.DepartureDate <= date2);

            var nowDate = DateTime.UtcNow;

            var activeOrders = new List<Order>();
            foreach (var order in orders)
            {
                if (order.State == OrderStateType.Canceled || order.State == OrderStateType.Failed)
                    continue;

                if (order.Source == OrderSource.Yandex &&
                    (order.State == OrderStateType.Created || order.State == OrderStateType.Assigned ||
                     order.State == OrderStateType.Accepted))
                    continue;

                if (order.State != OrderStateType.Completed)
                {
                    activeOrders.Add(order);
                }
            }

            List<OrderWithPriority> ordersWithoutDriver = new List<OrderWithPriority>();
            List<OrderWithPriority> executingOrders = new List<OrderWithPriority>();
            List<OrderWithPriority> yandexApprovedOrders = new List<OrderWithPriority>();
            List<OrderWithPriority> failedOrders = new List<OrderWithPriority>();
            List<OrderWithPriority> otherOrders = new List<OrderWithPriority>();

            var highPriorityTime = mGlobalSettingsRepository.List().First().HighPriorityOrderTime;

            ordersWithoutDriver = (from o in activeOrders
                                   where ((o.State == OrderStateType.Created || o.State == OrderStateType.Accepted ||
                                           o.State == OrderStateType.Assigned || o.State == OrderStateType.Approved) &&
                                          o.DriverId == null || o.State == OrderStateType.Rejected) &&
                                         (o.DepartureDate - nowDate).TotalMinutes < highPriorityTime
                                   select new OrderWithPriority(o, OrderPriority.HighPriority)).OrderBy(o => o.Order.DepartureDate)
                .ToList();

            executingOrders = (from o in activeOrders
                               where o.State == OrderStateType.Driving || o.State == OrderStateType.Transporting ||
                                     o.State == OrderStateType.Waiting
                               select new OrderWithPriority(o, OrderPriority.Normal)).OrderBy(o => o.Order.DepartureDate).ToList();

            yandexApprovedOrders = (from o in activeOrders
                                    where o.Source == OrderSource.Yandex && o.State == OrderStateType.Approved &&
                                          !ordersWithoutDriver.Any(oo => oo.Order.Id == o.Id)
                                    select new OrderWithPriority(o, OrderPriority.Normal)).OrderBy(o => o.Order.DepartureDate).ToList();

            failedOrders =
                (from o in activeOrders
                 where o.State == OrderStateType.Failed
                 select new OrderWithPriority(o, OrderPriority.Normal)).OrderBy(o => o.Order.DepartureDate).ToList();

            otherOrders = (from o in activeOrders
                           where
                               !ordersWithoutDriver.Any(oo => oo.Order.Id == o.Id) &&
                               !executingOrders.Any(oo => oo.Order.Id == o.Id) &&
                               !yandexApprovedOrders.Any(oo => oo.Order.Id == o.Id) && !failedOrders.Any(oo => oo.Order.Id == o.Id)
                           select new OrderWithPriority(o, OrderPriority.Normal)).OrderBy(o => o.Order.DepartureDate).ToList();


            var activesOrderWithPriorities = new List<OrderWithPriority>();

            activesOrderWithPriorities.AddRange(ordersWithoutDriver);
            activesOrderWithPriorities.AddRange(executingOrders);
            activesOrderWithPriorities.AddRange(yandexApprovedOrders);
            activesOrderWithPriorities.AddRange(otherOrders);
            activesOrderWithPriorities.AddRange(failedOrders);

            return activesOrderWithPriorities;
        }

        public IList<Order> GetOrders(long customerId, int take)
        {
            return mRepository.List(o => o.CustomerId != null && o.CustomerId == customerId, 0, take, "Id", SortOrder.Desc);
        }

        public void AddAddOrderHistory(Order order, string updateReason)
        {
            OrderHistory orderHistory = new OrderHistory(order);

            orderHistory.Comment = updateReason;
            var changedProperties = GetAddProperties(order);
            orderHistory.ChangedProperty = changedProperties.Properties;
            orderHistory.OldValues = changedProperties.OldValues.Substring(1);

            mOrderHistoryRepository.Add(orderHistory);
            mOrderHistoryRepository.Commit();
        }

        public void AddCancelOrderHistory(Order order, string updateReason)
        {
            if (mRepository.IsItemPropertyChanged(order, "State"))
            {
                OrderHistory orderHistory = new OrderHistory(order);
                orderHistory.Comment = updateReason;
                orderHistory.OldValues = OrderHistoryHelper.GetStateMessage(order.State, order.Id, order.Source,
                                                   order.Driver);
                orderHistory.ChangedProperty = orderHistory.ChangedProperty | OrderChangedProperties.State;
                if (order.State == OrderStateType.Failed || order.State == OrderStateType.Canceled)
                    orderHistory.MessageType = HistoryMessageType.Danger;
                mOrderHistoryRepository.Add(orderHistory);
                mOrderHistoryRepository.Commit();
            }
        }

        public void AddUpdateOrderHistory(Order order, string updateReason)
        {
            OrderHistory orderHistory = new OrderHistory(order);

            var changedProperties = GetChangedProperties(order);
            if (changedProperties.OldValues != "" && changedProperties.OldValues != "@")
            {
                orderHistory.Comment = updateReason;

                orderHistory.ChangedProperty = changedProperties.Properties;
                orderHistory.MessageType = changedProperties.MessageType;
                orderHistory.OldValues = changedProperties.OldValues == ""
                    ? changedProperties.OldValues
                    : changedProperties.OldValues.Substring(1);
                mOrderHistoryRepository.Add(orderHistory);
                mOrderHistoryRepository.Commit();
            }
        }

        private class ChangedProperties
        {
            public OrderChangedProperties Properties { get; set; }
            public string OldValues { get; set; }

            public HistoryMessageType MessageType { get; set; }

            public ChangedProperties()
            {
                Properties = OrderChangedProperties.None;
                OldValues = "";
            }
        }

        private ChangedProperties GetAddProperties(Order order)
        {
            var changedProperties = new ChangedProperties();

            if (order == null)
            {
                return changedProperties;
            }
            var newOrderId = (long)mRepository.GetNewValue(order, "Id");
            changedProperties.OldValues += "@" +
                                           OrderHistoryHelper.GetStateMessage(OrderStateType.Created, newOrderId,
                                               order.Source,
                                               order.Driver);
            if (order.State == OrderStateType.Assigned)
            {
                changedProperties.OldValues += "@" +
                                               OrderHistoryHelper.GetStateMessage(order.State, newOrderId, order.Source,
                                                   order.Driver);
            }

            changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.State;
            //if (order.State != OrderStateType.Assigned)
            //{

            //    Driver driver = null;
            //    if (order.DriverId != null)
            //    {
            //        driver = mDriverRepository.Single(d => d.Id == order.DriverId);
            //    }
            //    changedProperties.OldValues += "@" + OrderHistoryHelper.GetDriverMessage(driver);
            //    changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.DriverId;
            //}

            return changedProperties;
        }

        private ChangedProperties GetChangedProperties(Order order)
        {
            var changedProperties = new ChangedProperties();

            if (order == null)
            {
                return changedProperties;
            }
            changedProperties.MessageType = HistoryMessageType.Normal;

            if (mRepository.IsItemPropertyChanged(order, "State"))
            {
                if (order.State != OrderStateType.Created)
                {

                    changedProperties.OldValues += "@" +
                                                   OrderHistoryHelper.GetStateMessage(order.State, order.Id,
                                                       order.Source,
                                                       order.Driver);
                    changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.State;
                    if (order.State == OrderStateType.Disapproved)
                        changedProperties.MessageType = HistoryMessageType.Danger;
                    if (order.State == OrderStateType.Completed)
                        changedProperties.MessageType = HistoryMessageType.Success;
                }
            }
            if (mRepository.IsItemPropertyChanged(order, "DriverId") &&
                mRepository.GetOldValue(order, "DriverId") != null)
            {
                Driver driver = null;
                if (order.DriverId != null)
                {
                    driver = mDriverRepository.Single(d => d.Id == order.DriverId);
                }
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetDriverMessage(driver);

                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.DriverId;
                changedProperties.MessageType = HistoryMessageType.Warning;
            }
            var oldFromAddress = (Address)mRepository.GetOldValue(order, "FromAddress");
            if (oldFromAddress.FullName != order.FromAddress.FullName)
            {
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetAddressMessage(true, oldFromAddress.FullName);
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.FromAddress;
                changedProperties.MessageType = HistoryMessageType.Warning;
            }
            var oldToAddress = (Address)mRepository.GetOldValue(order, "ToAddress");
            if (oldToAddress.FullName != order.ToAddress.FullName)
            {
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetAddressMessage(false, oldToAddress.FullName);
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.ToAddress;
                changedProperties.MessageType = HistoryMessageType.Warning;
            }
            if (mRepository.IsItemPropertyChanged(order, "DepartureDate"))
            {
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetDepartureTimeMessage((DateTime)mRepository.GetOldValue(order, "DepartureDate"), order.DepartureDate);
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.DepartureDate;
                changedProperties.MessageType = HistoryMessageType.Warning;
            }
            if (mRepository.IsItemPropertyChanged(order, "Comments"))
            {
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetCommentMessage((string)mRepository.GetOldValue(order, "Comments"), order.Comments);
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.DepartureDate;
            }
            if (mCustomerRepository.IsItemPropertyChanged(order.Customer, "Phone") || mCustomerRepository.IsItemPropertyChanged(order.Customer, "Name"))
            {
                var oldCustomer = new Customer()
                {
                    Id = 0,
                    Name = (string)mCustomerRepository.GetOldValue(order.Customer, "Name"),
                    Phone = (string)mCustomerRepository.GetOldValue(order.Customer, "Phone")
                };
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetCustomerMessage(oldCustomer, order.Customer);
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.Customer;
                changedProperties.MessageType = HistoryMessageType.Warning;
            }
            if (mRepository.IsItemPropertyChanged(order, "Source"))
            {
                changedProperties.OldValues += "@" + OrderHistoryHelper.GetSourceMessage(order.Source, (OrderSource)mRepository.GetOldValue(order, "Source"));
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.Cost;
            }
            if (mRepository.IsItemPropertyChanged(order, "TariffId"))
            {
                var oldTarId = mRepository.GetOldValue(order, "TariffId");
                if (oldTarId != null)
                {
                    long tarId;
                    if (long.TryParse(oldTarId.ToString(), out tarId))
                    {
                        var tar = mTariffRepository.Single(d => d.Id == tarId);
                        changedProperties.OldValues += "@" + OrderHistoryHelper.GetTariffMessage(order.Tariff, tar);
                    }
                }

                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.TariffId;
            }
            if (mRepository.IsItemPropertyChanged(order, "OrderOptions.CarFeatures"))
            {
                changedProperties.OldValues += "@" +
                                               OrderHistoryHelper.GetCategoriesMessage(order.OrderOptions.CarFeatures,
                                                   (CarFeatures)
                                                       mRepository.GetOldValue(order, "OrderOptions.CarFeatures"));
                changedProperties.OldValues += "@" +
                               OrderHistoryHelper.GetServicesMessage(order.OrderOptions.CarFeatures,
                                   (CarFeatures)
                                       mRepository.GetOldValue(order, "OrderOptions.CarFeatures"));
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.CarFeatures;
            }
            if (mRepository.IsItemPropertyChanged(order, "OrderOptions.ChildrenSeat"))
            {
                changedProperties.OldValues += "@" +
                                               OrderHistoryHelper.GetChildrenSeatMessage(
                                                   order.OrderOptions.ChildrenSeat,
                                                   (ChildrenSeat)
                                                       mRepository.GetOldValue(order, "OrderOptions.ChildrenSeat"));
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.ChildrenSeat;
            }
            if (mRepository.IsItemPropertyChanged(order, "Discount"))
            {
                changedProperties.OldValues += "@" +
                                               OrderHistoryHelper.GetOrderdiscountMessage(order.Discount,
                                                   (byte)mRepository.GetOldValue(order, "Discount"));
                changedProperties.Properties = changedProperties.Properties | OrderChangedProperties.Discount;
            }

            return changedProperties;
        }

        public OrderHistory GetLastStateTime(long orderId, OrderStateType orderState)
        {
            OrderHistory orderHistory =
                mOrderHistoryRepository.List(oh => oh.OrderId == orderId && oh.OrderState == orderState)
                    .LastOrDefault();
            return orderHistory;
        }

        public Order GetOrder(string sourceOrderId)
        {
            return mRepository.Single(o => o.SourceOrderId == sourceOrderId);
        }

        public Order GetDriverCurrentOrder(long driverId)
        {
            var order =
                mRepository.List(
                    o =>
                        o.DriverId == driverId &&
                        (o.State != OrderStateType.Created && o.State != OrderStateType.Accepted &&
                         o.State != OrderStateType.Approved && o.State != OrderStateType.Disapproved &&
                         o.State != OrderStateType.Completed)).OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                order = mRepository.List(o => o.DriverId == driverId).LastOrDefault();
            }
            return order;
        }

        public Order GetCustomerCurrentOrder(long customerId)
        {
            var order =
                mRepository.List(
                    o =>
                        o.CustomerId == customerId &&
                        (o.State != OrderStateType.Created && o.State != OrderStateType.Accepted &&
                         o.State != OrderStateType.Approved && o.State != OrderStateType.Disapproved &&
                         o.State != OrderStateType.Completed)).OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                order = mRepository.List(o => o.CustomerId == customerId).LastOrDefault();
            }
            return order;
        }

        public List<LightOrderWithPriority> GetLightOrders()
        {
            var orders = GetActiveOrders();

            List<LightOrderWithPriority> report = GetLightOrders(orders);
            
            return report;
        }

        public List<LightOrderWithPriority> GetLightOrders(IEnumerable<OrderWithPriority> orders)
        {
            List<LightOrderWithPriority> report = new List<LightOrderWithPriority>();
            LightOrderWithPriority reportItem;
            var orderIds = orders.Select(o => o.Order.Id).ToList();
            var histories = mOrderHistoryRepository.List(o => orderIds.Contains(o.OrderId));

            var ordersAndHistory = orders.GroupJoin(histories, o => o.Order.Id, h => h.OrderId, (o, h) => new
            {
                Order = o,
                LastHistory =
                    h.Where(hi => hi.ChangedProperty.HasFlag(OrderChangedProperties.State))
                        .OrderByDescending(hi => hi.Timestamp)
            }).ToList();

            var customersIds = ordersAndHistory.Select(o => o.Order.Order.CustomerId).ToList();
            var customers = mCustomerRepository.List(c => customersIds.Contains(c.Id));

            var ordersAndHistoryAndCus = ordersAndHistory.Join(customers, o => o.Order.Order.CustomerId, c => c.Id,
                (o, c) => new
                {
                    oh = o,
                    cus = c
                });

            foreach (var orderWhistory in ordersAndHistoryAndCus)
            {
                reportItem = new LightOrderWithPriority();
                reportItem.Order.Id = orderWhistory.oh.Order.Order.Id;
                reportItem.Order.FromAddress = orderWhistory.oh.Order.Order.FromAddress.FullName;
                reportItem.Order.ToAddress = orderWhistory.oh.Order.Order.ToAddress.FullName;
                reportItem.Order.DepartureDate = TimeConverter.UtcToLocal(orderWhistory.oh.Order.Order.DepartureDate);
                reportItem.Order.Cost = orderWhistory.oh.Order.Order.Cost;

                if (orderWhistory.oh.Order.Order.Source == OrderSource.Yandex)
                {
                    reportItem.Order.IsYandex = true;
                }

                if (orderWhistory.oh.Order.Order.Driver != null)
                {
                    Driver driver = orderWhistory.oh.Order.Order.Driver;
                    reportItem.Order.DriverId = driver.Id;
                    reportItem.Order.DriverName = String.Format("{0} {1} {2}", driver.LastName, driver.FirstName,
                        driver.MiddleName);
                    reportItem.Order.DriverPhone = driver.Phones;
                    if (driver.Car != null)
                    {
                        reportItem.Order.DriverCallsign = driver.Car.Callsign;
                    }
                }
                else
                {
                    reportItem.Order.DriverName = null;
                    reportItem.Order.DriverCallsign = null;
                    reportItem.Order.DriverPhone = null;
                }
                if (orderWhistory.cus != null)
                {
                    reportItem.Order.CustomerName = orderWhistory.cus.Name;
                    reportItem.Order.CustomerPhone = orderWhistory.cus.Phone;

                    reportItem.Order.IsOrganization = false;
                    if (orderWhistory.cus.OrganizationId != null)
                    {
                        reportItem.Order.IsOrganization = true;
                    }
                }
                else
                {
                    reportItem.Order.CustomerName = null;
                    reportItem.Order.CustomerPhone = null;
                }
                reportItem.Order.IsFreeWaitOver = false;
                reportItem.Order.State = orderWhistory.oh.Order.Order.State;
                reportItem.Order.LastHistoryTime = TimeConverter.UtcToLocal(orderWhistory.oh.LastHistory.First().Timestamp);
                if (orderWhistory.oh.Order.Order.Tariff != null)
                {
                    reportItem.Order.TariffName = orderWhistory.oh.Order.Order.Tariff.Abbreviation;

                    if (reportItem.Order.State == OrderStateType.Waiting)
                    {
                        var startWaitingHistory = orderWhistory.oh.LastHistory.FirstOrDefault(h => h.OrderState == OrderStateType.Waiting);

                        if (startWaitingHistory != null)
                        {
                            var currTime = DateTime.UtcNow;
                            var startWaitingTime = startWaitingHistory.Timestamp;
                            if ((currTime - startWaitingTime).TotalSeconds > orderWhistory.oh.Order.Order.Tariff.FreeWaitingMinutes * 60)
                            {
                                reportItem.Order.IsFreeWaitOver = true;
                            }
                        }
                    }
                }
                else
                {
                    reportItem.Order.TariffName = null;
                }
                reportItem.Priority = orderWhistory.oh.Order.Priority;
                report.Add(reportItem);
            }

            return report;
        }

        public double CalculateOrderCost(OrderPathWithTariff orderPath)
        {
            var tariff = mTariffRepository.Single(t => t.Id == orderPath.TariffId);
            if (tariff == null)
                return 0.0;

            var fromAddress = new Address()
            {
                Latitude = orderPath.Segments.First().Latitude,
                Longitude = orderPath.Segments.First().Longitude,
            };
            var toAddress = new Address()
            {
                Latitude = orderPath.Segments.Last().Latitude,
                Longitude = orderPath.Segments.Last().Longitude,
            };
            var fixedCost = TaximeterHelper.GetMinCost(fromAddress, toAddress, tariff, mGeocodeService,
                mRegionBinaryStorage);

            if (fixedCost > 0.0)
                return fixedCost;

            var regionPoints = mRegionBinaryStorage.GetAllPoints();
            var pointsByRegion = regionPoints.GroupBy(rp => rp.RegionId);

            return TaximeterHelper.GetOrderCost(orderPath, tariff, pointsByRegion);
        }


        private void AddOrderStateChangedEvent(Order order, bool force = false)
        {
            if (mRepository.IsItemPropertyChanged(order, "State") || force)
            {
                var sysEvent = new SystemEvent
                {
                    RelatedItemId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = EventType.OrderStateChanged,
                    OrderState = order.State
                };

                mEventRepository.Add(sysEvent);
            }

            if (mRepository.IsItemPropertyChanged(order, "DriverId") &&
                mRepository.GetOldValue(order, "DriverId") != null)
            {
                var sysEvent = new SystemEvent
                {
                    RelatedItemId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Type = EventType.DriverReplaced,
                    OrderState = order.State
                };

                mEventRepository.Add(sysEvent);
            }
        }

        private void SetOrderMinCost(Order order)
        {
            if (!string.IsNullOrEmpty(order.FromAddress.FullName) &&
                !string.IsNullOrEmpty(order.ToAddress.FullName) &&
                order.Tariff != null)
            {
                var minCost = TaximeterHelper.GetMinCost(order.FromAddress, order.ToAddress,
                    order.Tariff, mGeocodeService, mRegionBinaryStorage);
                mLogger.Debug("SetOrderMinCost to {0} for {1}({2})", minCost, order.SourceOrderId, order.Id);
                order.MinCost = minCost;
            }
        }

        /// <summary>
        /// Get driver's last order
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public Order GetLastOrder(long driverId)
        {
            var orders = mRepository.List(o => o.DriverId == driverId &&
                (o.State == OrderStateType.Completed || o.State == OrderStateType.Failed)
                , 0, 1, "DepartureDate", SortOrder.Desc);
            if (orders.Count > 0)
            {
                return orders[0];
            }
            return null;
        }

        public void AssigneDrivers(long orderId, List<long> driversIds)
        {
            mRepository.LockDrivers(driversIds, orderId);
            mRepository.Commit();
        }

        public void UnnasigneDrivers(long orderId, List<long> unassignedDrivers)
        {
            mRepository.UnlockDrivers(orderId, unassignedDrivers);
            mRepository.Commit();
        }

        public List<long> GetUnassignedDrivers()
        {
            var orderDrivers = mRepository.GetAssignedDrivers();
            var unassignedDrivers = mDriverRepository
                .List(d => !orderDrivers.Contains(d.Id))
                .Select(d => d.Id)
                .ToList();
            return unassignedDrivers;
        }

        public List<Order> GetFeedbackOrders(List<string> sourceOrderIds)
        {
            return mRepository.List().Where(o => sourceOrderIds.Contains(o.SourceOrderId)).ToList();
        }

        public void Dispose()
        {
            mCustomerRepository.Dispose();
            mDriverRepository.Dispose();
            mEventRepository.Dispose();
            mGlobalSettingsRepository.Dispose();
            mOrderHistoryRepository.Dispose();
            mRegionService.Dispose();
            mRepository.Dispose();
            mTariffRepository.Dispose();
            mTransactionService.Dispose();
        }
    }
}
