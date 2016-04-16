using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.Ioc;
using Dexpa.OrdersGateway;
using Dexpa.ServiceCore;
using Newtonsoft.Json;
using Yandex.Taxi.Gateway.Contracts;
using Yandex.Taxi.Model.Orders;
using Order = Dexpa.Core.Model.Order;

namespace Dexpa.YandexTaxiService
{
    public class YaTaxiService : AService
    {
        private DateTime mLastRequestTime;

        private DateTime mLastOrderStateTime;

        private DateTime mLastDriverReplacedTime;

        private IGateway mGateway;

        private readonly int mPauseBeforeNextOrder;

        private readonly int mOrderRequestAdditionalTime;

        private OrderProcessor mOrderProcessor;

        public YaTaxiService(IGateway gateway, int pauseBeforeNextOrder, int orderRequestAdditionalTime)
        {
            mGateway = gateway;
            mPauseBeforeNextOrder = pauseBeforeNextOrder;
            mOrderRequestAdditionalTime = orderRequestAdditionalTime;
            mIterationPauseMs = 1000;
            mLastRequestTime = DateTime.UtcNow;
            mLastOrderStateTime = DateTime.UtcNow;
            mLastDriverReplacedTime = DateTime.UtcNow;
            mOrderProcessor = new OrderProcessor(mPauseBeforeNextOrder, mOrderRequestAdditionalTime);
        }

        protected override void BeforeStart()
        {
            mLastRequestTime = DateTime.UtcNow;
            mLastOrderStateTime = DateTime.UtcNow;
            mLastDriverReplacedTime = DateTime.UtcNow;
            mLogger.Debug("Listening orders from {0}", mLastRequestTime);
            base.BeforeStart();
        }

        protected override void WorkIteration()
        {
            using (var context = new OperationContext())
            {
                CheckNewRequests(context);
                CheckNewOrderStates(context);
                CheckDriverReplaced(context);
            }
        }

        private void CheckDriverReplaced(OperationContext context)
        {
            var eventService = context.EventService;
            var events = eventService.GetDriverReplacedEvents(mLastDriverReplacedTime);
            var yandexEvents = events.Where(a => a.Order.Source == OrderSource.Yandex);
            foreach (var systemEvent in yandexEvents)
            {
                try
                {
                    var order = systemEvent.Order;
                    var orderStateToYa = OrderStateToYa(order.State);
                    if (orderStateToYa.HasValue)
                    {
                        mLogger.Debug("Sent to yandex driver for order {0}({1}) replaced to {2}", order.SourceOrderId, order.Id, order.Driver.Id);
                        mGateway.SendOrderUpdate(order.SourceOrderId, orderStateToYa.Value, null, order.Driver.Uuid);
                    }
                }
                catch (Exception exception)
                {
                    mLogger.Error("Error", exception);
                }
            }
            if (events.Count > 0)
            {
                mLastDriverReplacedTime = events.Last().Timestamp;
            }
        }

        private void CheckNewOrderStates(OperationContext context)
        {
            var eventService = context.EventService;
            var events = eventService.GetOrderStateChangedEvents(mLastOrderStateTime);
            var yandexEvents = events.Where(a => a.Order.Source == OrderSource.Yandex);
            foreach (var systemEvent in yandexEvents)
            {
                try
                {
                    Order order = systemEvent.Order;
                    switch (systemEvent.OrderState)
                    {
                        case OrderStateType.Driving:
                            string driverUuid = order.Driver.Uuid;
                            mGateway.SendOrderUpdate(order.SourceOrderId, OrderStatus.Driving, driverUuid);
                            mLogger.Debug("Driving status sent to yandex. Driver {0}, order {1}", driverUuid, order.SourceOrderId);
                            break;
                        case OrderStateType.Waiting:
                            mGateway.SendOrderUpdate(order.SourceOrderId, OrderStatus.Waiting);
                            mLogger.Debug("Waiting status sent to yandex. Order {0}", order.SourceOrderId);
                            break;
                        case OrderStateType.Transporting:
                            mGateway.SendOrderUpdate(order.SourceOrderId, OrderStatus.Transporting);
                            mLogger.Debug("Transporting status sent to yandex. Order {0}", order.SourceOrderId);
                            break;
                        case OrderStateType.Completed:
                            string cost = order.Cost.ToString(CultureInfo.InstalledUICulture);
                            mGateway.SendOrderUpdate(order.SourceOrderId, OrderStatus.Complete, cost);
                            mLogger.Debug("Completed status sent to yandex. Order {0}. Cost {1}", order.SourceOrderId, cost);
                            if (order.Driver.State == DriverState.Busy)
                            {
                                mLogger.Debug("Wrong driver {0} state. Changed from Busy to ReadyToWork", order.Driver.Id);
                                var driver = order.Driver;
                                driver.State = DriverState.ReadyToWork;
                                context.DriverService.UpdateDriver(driver);
                            }
                            break;
                        case OrderStateType.Failed:
                            mGateway.SendOrderUpdate(order.SourceOrderId, OrderStatus.Failed);
                            mLogger.Debug("Failed status sent to yandex. Order {0}.", order.SourceOrderId);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    mLogger.Error("Error", exception);
                }
            }
            if (events.Count > 0)
            {
                mLastOrderStateTime = events.Last().Timestamp;
            }
        }

        private static OrderStatus? OrderStateToYa(OrderStateType orderStateType)
        {
            switch (orderStateType)
            {
                case OrderStateType.Driving:
                    return OrderStatus.Driving;
                case OrderStateType.Waiting:
                    return OrderStatus.Waiting;
                case OrderStateType.Transporting:
                    return OrderStatus.Transporting;
                case OrderStateType.Completed:
                    return OrderStatus.Complete;
                case OrderStateType.Failed:
                    return OrderStatus.Failed;
                default:
                    return null;
            }
        }

        private void CheckNewRequests(OperationContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IList<OrderRequest> requests;
            requests = context.OrderRequestService.GetOrderRequests(mLastRequestTime);
            if (requests.Count > 0)
            {
                mLogger.Debug("Founded new requests: {0}", requests.Count);
            }
            foreach (var orderRequest in requests)
            {
                try
                {
                    if (orderRequest.Type == OrderRequestType.New)
                    {
                        ProcessRequestNew(context, orderRequest);
                    }
                    else if (orderRequest.Type == OrderRequestType.Cancelled)
                    {
                        ProcessRequestCancel(context, orderRequest);
                    }
                    else
                    {
                        throw new Exception("Unknown request type: " + orderRequest.Type + ", request id=" + orderRequest.Id);
                    }
                }
                catch (Exception exception)
                {
                    mLogger.Error(exception);
                }
            }
            if (requests.Count > 0)
            {
                mLastRequestTime = requests.Last().Timestamp;
                context.OrderRequestService.RemoveRequests(requests);
            }

            stopwatch.Stop();
            if (requests.Count > 0)
            {
                mLogger.Debug("Requests process time: {0} ms", stopwatch.ElapsedMilliseconds);
            }
        }

        private void ProcessRequestCancel(OperationContext context, OrderRequest orderRequest)
        {
            mLogger.Debug("Cancel request: {0}", orderRequest.OrderUid);
            var orderService = context.OrderService;
            var order = orderService.GetOrder(orderRequest.OrderUid);
            if (order != null)
            {
                order.State = OrderStateType.Canceled;
                orderService.UpdateOrder(order, "Yandex отменил заказ. Причина: " + orderRequest.DataJson);

                var requests = context.DriverOrderRequestService.GetRequests(order.Id);
                context.DriverOrderRequestService.RemoveRequests(requests);
            }
            else
            {
                mLogger.Error("Order is null: {0}", orderRequest.OrderUid);
            }
        }

        private void ProcessRequestNew(OperationContext context, OrderRequest orderRequest)
        {
            var yaOrder = JsonConvert.DeserializeObject<Yandex.Taxi.Model.Orders.Order>(orderRequest.DataJson);
            Order order;
            using (new OperationStopwatch("Get order by id"))
            {
                order = context.OrderService.GetOrder(yaOrder.Id);
            }
            if (order == null)
            {
                order = CreateOrder(yaOrder, context.TariffsService);
                using (new OperationStopwatch("Add new order"))
                {
                    context.OrderService.AddOrder(order);
                }
            }
            mLogger.Debug("New order: {0}({1}) {2} {3}, tariff: {4}, options: {5}, {6} to {7}", order.SourceOrderId,
                order.Id, order.FromAddress, order.ToAddress, order.TariffId, order.OrderOptions.CarFeatures,
                order.OrderOptions.ChildrenSeat, order.DepartureDate);

            IList<RobotLog> robotLogs;
            var driversIds = mOrderProcessor.ProcessOrder(yaOrder, context, order, out robotLogs);

            if (driversIds.Count > 0)
            {
                var drivers = context.DriverService.GetDrivers(includeFiredDrivers: false);
                var selectedDrivers = drivers
                    .Where(d => driversIds.Contains(d.Id))
                    .ToList();
                context.OrderService.AssigneDrivers(order.Id, driversIds);

                var assignedDrivers = new List<long>(driversIds.Count);
                using (new OperationStopwatch("Send ready to process to yandex"))
                {
                    var succesessRequests = 0;
                    try
                    {
                        foreach (var driver in selectedDrivers)
                        {
                            var result = mGateway.SendReadyToProceedOrder(driver.Uuid, yaOrder.Id);
                            if (result == ReadyToProceedOrderResult.Ok)
                            {
                                succesessRequests++;
                            }
                            mLogger.Debug("Ready to process sent for order {0} driver {1}. Result: {2}",
                                driver.Uuid, yaOrder.Id, result);
                            assignedDrivers.Add(driver.Id);
                        }
                    }
                    catch (Exception exception)
                    {
                        mLogger.Error(exception);
                        var unassignedDrivers = driversIds
                            .Where(d => !assignedDrivers.Contains(d))
                            .ToList();
                        context.OrderService.UnnasigneDrivers(order.Id, unassignedDrivers);
                        CancelOrder(context, order);
                        throw;
                    }

                    if (succesessRequests == 0)
                    {
                        CancelOrder(context, order);
                    }
                }
            }

            if (robotLogs.Count > 0)
            {
                using (new OperationStopwatch("Saving robot logs time"))
                {
                    context.RobotLogService.AddLogs(robotLogs);
                    mLogger.Debug("Robot logs added: {0}", robotLogs.Count);
                }
            }
        }

        private static void CancelOrder(OperationContext context, Order order)
        {
            order.State = OrderStateType.Canceled;
            string reason = "Принудительная отмена. Произошла ошибка при обработке заказа.";
            context.OrderService.UpdateOrder(order, reason);
        }

        private Order CreateOrder(Yandex.Taxi.Model.Orders.Order yaOrder, ITariffsService tariffsService)
        {
            using (new OperationStopwatch("CreateOrder"))
            {
                var orderOptions = GetOrderOptions(yaOrder);
                var tariff = GetOrderTariff(yaOrder, tariffsService);
                long? tariffId = tariff != null ? tariff.Id : (long?)null;

                var localityFrom = yaOrder.Source.Country.Locality;
                var thoroughfareFrom = localityFrom != null ? localityFrom.Thoroughfare : null;

                var staircase = "";
                var fullAddr = yaOrder.Source.FullName == "" ? "" : yaOrder.Source.FullName.ToLower();
                var splited = fullAddr.Split(new string[] { "подъезд " }, StringSplitOptions.None);
                if (splited.Length>1)
                {
                    staircase = splited[1];
                }

                Locality localityTo = null;
                Destination destination = null;
                if (yaOrder.Destinations.Count > 0)
                {
                    localityTo = yaOrder.Destinations[0].Country.Locality;
                    destination = yaOrder.Destinations[0];
                }
                var thoroughfareTo = localityTo != null ? localityTo.Thoroughfare : null;
                var fromPoint = yaOrder.Source.Point;
                var order = new Order
                {
                    DepartureDate = TimeConverter.LocalToUtc(yaOrder.BookingTime.Time),
                    Comments = yaOrder.Comments,
                    FromAddress = new Address
                    {
                        City = localityFrom != null ? localityFrom.Name : "",
                        Street = thoroughfareFrom != null ? thoroughfareFrom.Name : "",
                        House = thoroughfareFrom != null && thoroughfareFrom.Premise != null ? thoroughfareFrom.Premise.Number : "",
                        Staircase = staircase,
                        Latitude = fromPoint != null ? (double)fromPoint.Latitude : 0,
                        Longitude = fromPoint != null ? (double)fromPoint.Longitude : 0,
                        IsAirport = yaOrder.Source != null && IsAirport(yaOrder.Source.ShortName)
                    },
                    ToAddress = new Address
                    {
                        City = localityTo != null ? localityTo.Name : "",
                        Street = thoroughfareTo != null ? thoroughfareTo.Name : "",
                        House = thoroughfareTo != null && thoroughfareTo.Premise != null ? thoroughfareTo.Premise.Number : "",
                        Latitude = destination != null && destination.Point != null ? (double)destination.Point.Latitude : 0,
                        Longitude = destination != null && destination.Point != null ? (double)destination.Point.Longitude : 0,
                        IsAirport = destination != null && IsAirport(destination.ShortName)
                    },
                    Source = OrderSource.Yandex,
                    SourceOrderId = yaOrder.Id,
                    OrderOptions = orderOptions,
                    TariffId = tariffId
                };
                DoDomodedovoHack(order.FromAddress);
                DoDomodedovoHack(order.ToAddress);
                return order;
            }
        }

        private bool IsAirport(string shortName)
        {
            var keywords = new[] { "аэропорт", "шереметьево", "домодедово", "внуково" };
            return shortName != null && keywords.Any(word => shortName.ToLower().Contains(word));
        }

        private void DoDomodedovoHack(Address address)
        {
            if (address != null && address.Street != null &&
                address.Street.ToLower().Contains("домодедово"))
            {
                address.City = string.Empty;
            }
        }

        private Tariff GetOrderTariff(Yandex.Taxi.Model.Orders.Order yaOrder, ITariffsService tariffsService)
        {
            using (new OperationStopwatch("GetOrderTariff"))
            {
                if (yaOrder.Tariffs == null || yaOrder.Tariffs.Count == 0)
                {
                    mLogger.Error("Yandex sent order with no any tariffs");
                    return null;
                }
                else
                {
                    var tariffId = yaOrder.Tariffs[0];
                    var departureTime = TimeConverter.LocalToUtc(yaOrder.BookingTime.Time);
                    var tariff = tariffsService.GetActiveTariff(tariffId, departureTime);
                    if (tariff == null)
                    {
                        mLogger.Debug("There are no tariffs for {0}", tariffId);
                    }
                    return tariff;
                }
            }
        }

        private static OrderOptions GetOrderOptions(Yandex.Taxi.Model.Orders.Order yaOrder)
        {
            var noConstant = "no";
            var carFeatures = CarFeatures.None;
            var childSeat = ChildrenSeat.None;
            foreach (var requirement in yaOrder.Requirements)
            {
                var value = requirement.Value.ToLower();
                if (requirement.Type == RequirementType.ChildChair)
                {
                    if (value != noConstant)
                    {
                        int fromYear, fromMonth, toYear, toMonth;
                        var ageRange = requirement.Value.Split('-');

                        var fromAge = ageRange[0].Split('.');
                        fromYear = int.Parse(fromAge[0]);
                        fromMonth = int.Parse(fromAge.Length > 1 ? fromAge[1] : "0");

                        if (ageRange.Length == 2)
                        {
                            var toAge = ageRange[1].Split('.');

                            toYear = int.Parse(toAge[0]);
                            toMonth = int.Parse(toAge.Length > 1 ? toAge[1] : "0");
                        }
                        else
                        {
                            toYear = fromYear;
                            toMonth = fromMonth;
                        }


                        if (fromYear == 0 && fromMonth >= 0 && toYear <= 1)
                        {
                            childSeat = ChildrenSeat.Weight0_10;
                        }
                        else if (fromYear == 0 && fromMonth >= 0 && toYear <= 2)
                        {
                            childSeat = ChildrenSeat.Weight0_13;
                        }
                        else if (fromYear == 0 && fromMonth >= 0 && toYear <= 5)
                        {
                            childSeat = ChildrenSeat.Weight0_20;
                        }
                        else if (fromYear == 0 && fromMonth >= 0 && toYear <= 7)
                        {
                            childSeat = ChildrenSeat.Weight0_25;
                        }
                        else if (fromYear == 0 && fromMonth >= 0 && toYear <= 12)
                        {
                            childSeat = ChildrenSeat.Weight0_40;
                        }
                        else if (fromYear == 1 && toYear <= 4)
                        {
                            childSeat = ChildrenSeat.Weight15_25;
                        }
                        else if (fromYear == 1 && toYear <= 10)
                        {
                            childSeat = ChildrenSeat.Weight22_36;
                        }
                        else if (fromYear == 3 && toYear <= 7)
                        {
                            childSeat = ChildrenSeat.Weight9_18;
                        }
                        else if (fromYear == 6 && toYear <= 10)
                        {
                            childSeat = ChildrenSeat.Weight9_36;
                        }
                    }
                }

                if (value != noConstant)
                {
                    switch (requirement.Type)
                    {
                        case RequirementType.AnimalTransport:
                            carFeatures |= CarFeatures.WithAnimals;
                            break;
                        case RequirementType.Check:
                            break;
                        case RequirementType.HasConditioner:
                            carFeatures |= CarFeatures.Conditioner;
                            break;
                        case RequirementType.Universal:
                            carFeatures |= CarFeatures.StationWagon;
                            break;
                        case RequirementType.YandexMoney:
                            break;
                        case RequirementType.Coupon:
                            break;
                    }
                }
                if (value == noConstant && requirement.Type == RequirementType.NoSmoking)
                {
                    carFeatures |= CarFeatures.Smoke;
                }
            }

            if (yaOrder.Tariffs.Contains("econom"))
            {
                carFeatures |= CarFeatures.Economy;
            }
            if (yaOrder.Tariffs.Contains("vip"))
            {
                carFeatures |= CarFeatures.Bussiness;
            }
            if (yaOrder.Tariffs.Contains("business"))
            {
                carFeatures |= CarFeatures.Comfort;
            }

            var orderOptions = new OrderOptions
            {
                CarFeatures = carFeatures,
                ChildrenSeat = childSeat
            };
            return orderOptions;
        }
    }
}
