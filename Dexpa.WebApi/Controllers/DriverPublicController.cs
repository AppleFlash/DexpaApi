using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Events;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.DTO.Events;
using Dexpa.WebApi.Models;
using Dexpa.WebApi.Utils;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Dexpa.WebApi.Controllers
{
    [Authorize]
    public class DriverPublicController : ApiControllerBase
    {
        private IDriverService mDriverService;

        private IOrderService mOrderService;

        private IEventService mEventService;

        private ITariffsService mTariffsService;

        private ITransactionService mTransactionService;

        private IDriverOrderRequestService mDriverOrderRequestService;

        public DriverPublicController(IDriverService driverService,
            IOrderService orderService,
            IEventService eventService,
            ITariffsService tariffsService,
            ITransactionService transactionService,
            IDriverOrderRequestService driverOrderRequestService)
        {
            mDriverService = driverService;
            mOrderService = orderService;
            mEventService = eventService;
            mTariffsService = tariffsService;
            mTransactionService = transactionService;
            mDriverOrderRequestService = driverOrderRequestService;
        }

        private DriverResult DriverResult(HttpStatusCode status, Error error)
        {
            return new DriverResult(status, Request, error);
        }

        [Route("api/driver/orders/urgent")]
        [HttpGet]
        public IHttpActionResult GetUrgentOrderRequests()
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var from = DateTime.UtcNow;
                var to = from.AddDays(1);
                var orders = mOrderService.GetOrders(from, to, driver.Id, OrderStateType.Approved);
                if (orders.Count == 0)
                {
                    var newOrders = new List<Order>(mOrderService.GetOrders(from, to, null, OrderStateType.Created));
                    var rejectedOrders = mOrderService.GetOrders(from, to, null, OrderStateType.Rejected);
                    newOrders.AddRange(rejectedOrders);

                    newOrders = newOrders.Where(o => o.Source != OrderSource.Yandex).ToList();

                    var checkResult = new List<RobotLog>();

                    var currentTime = DateTime.UtcNow;
                    foreach (var order in newOrders)
                    {
                        var orderDistance = Core.Utils.Utils.GetDistance(driver.Location.Latitude,
                            driver.Location.Longitude,
                            order.FromAddress.Latitude, order.FromAddress.Longitude);
                        var orderTime = (order.DepartureDate - currentTime).TotalMinutes;

                        //Здесь лог используется не по назначению. Нужно обобщить и разделить его на две модели
                        var log = new RobotLog
                        {
                            DriverId = driver.Id,
                            Driver = driver,
                            OrderId = order.Id,
                            Order = order,
                            IsDriverOptionsFit = driver.IsFitOrder(order.OrderOptions),
                            IsDriverWorkAllowed = driver.IsWorkAllowed(),
                            OrderDistance = orderDistance,
                            OrderTime = orderTime,
                            RobotDistance = driver.RobotSettings.OrderRadius,
                            RobotTime = driver.RobotSettings.MinutesDepartureTime,
                            RobotEnabled = driver.RobotSettings.Enabled,
                            DriverState = driver.State,
                            IsDriverOnline = driver.IsOnline
                        };

                        checkResult.Add(log);
                    }

                    var fitOrders = checkResult
                        .Where(r => r.Verdict == RobotVerdict.Fit)
                        .OrderByDescending(r => r.OrderTime)
                        .ToList();

                    orders = new List<Order>();
                    foreach (var order in fitOrders)
                    {
                        var request = mDriverOrderRequestService.GetRequest(driver.Id, order.OrderId);
                        if (request != null && request.State == OrderRequestState.Rejected)
                        {
                            continue;
                        }
                        orders.Add(order.Order);
                    }
                }

                var ordersDto = ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(orders);
                ProcessOrders(orders, ordersDto);
                return Ok(ordersDto);
            }
            return Unauthorized();
        }

        [Route("api/driver/orders/actual")]
        [HttpGet]
        public IHttpActionResult GetActualOrder()
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var toDate = DateTime.UtcNow.AddDays(60);
                var fromDate = DateTime.UtcNow.AddDays(-5);

                var orders = mOrderService.GetOrders(fromDate, toDate, driver.Id);

                var actualOrder = orders.FirstOrDefault(o => o.Source != OrderSource.Yandex && (o.State == OrderStateType.Created || o.State == OrderStateType.Assigned) ||
                                                    o.State == OrderStateType.Accepted ||
                                                    o.State == OrderStateType.Approved ||
                                                    o.State == OrderStateType.Driving ||
                                                    o.State == OrderStateType.Waiting ||
                                                    o.State == OrderStateType.Transporting);

                if (actualOrder == null)
                {
                    return NotFound();
                }

                var ordersDto = ObjectMapper.Instance.Map<Order, OrderDTO>(actualOrder);
                ProcessOrder(actualOrder, ordersDto);
                return Ok(ordersDto);
            }
            return Unauthorized();
        }


        [Route("api/driver/orders")]
        [HttpGet]
        public IHttpActionResult GetOrder(long orderId)
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var order = mOrderService.GetOrder(orderId);
                if (order == null)
                {
                    return NotFound();
                }
                var ordersDto = ObjectMapper.Instance.Map<Order, OrderDTO>(order);
                ProcessOrder(order, ordersDto);
                return Ok(ordersDto);
            }
            return Unauthorized();
        }

        [Route("api/driver/orders")]
        [HttpGet]
        public IHttpActionResult Get(DateTime? fromDate = null, DateTime? toDate = null, string orderState = null)
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                if (!fromDate.HasValue && !toDate.HasValue)
                {
                    fromDate = DateTime.UtcNow;
                    toDate = fromDate.Value.AddMinutes(40);
#if TEST_ENV
                    toDate = fromDate.Value.AddDays(30);
#endif
                }
                else
                {
                    toDate = TimeConverter.LocalToUtc(toDate.Value);
                    fromDate = TimeConverter.LocalToUtc(fromDate.Value);
                }

                var orderStateType = ParseOrderState(orderState);
                IList<Order> orders;
                if (orderStateType == OrderStateType.Created)//For 'Free orders'
                {
                    orders = mOrderService.GetOrders(fromDate.Value, toDate.Value, OrderStateType.Created);

                    //Adding rejected orders by other drivers
                    var rejectedByOtherDrivers = mOrderService.GetOrders(fromDate.Value, toDate.Value, driver.Id, OrderStateType.Rejected);

                    rejectedByOtherDrivers = rejectedByOtherDrivers.Where(o => o.DriverId != driver.Id).ToList();
                    rejectedByOtherDrivers.ForEach(o => orders.Add(o));

                    orders = orders.DistinctBy(o => o.Id).ToList();

                    var fitOrders = new List<Order>();
                    foreach (var order in orders)
                    {
                        if (driver.IsFitOrder(order.OrderOptions))
                        {
                            fitOrders.Add(order);
                        }
                    }

                    orders = fitOrders;

                    orders = orders.Where(o => o.Source != OrderSource.Yandex).ToList();//Do not show yandex orders.
                }
                else//For 'My orders'
                {
                    orders = mOrderService.GetOrders(fromDate.Value, toDate.Value, driver.Id);
                    orders = orders
                        .Where(o => o.State == OrderStateType.Assigned || o.State == OrderStateType.Approved)
                        .ToList();
                }

                orders = orders.OrderBy(o => o.DepartureDate).ToList();
                var ordersDto = ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(orders);
                ProcessOrders(orders, ordersDto);
                return Ok(ordersDto);
            }
            return Unauthorized();
        }

        private static OrderStateType? ParseOrderState(string orderState)
        {
            OrderStateType? orderStateType = null;
            if (!string.IsNullOrEmpty(orderState))
            {
                OrderStateType result;
                if (Enum.TryParse(orderState, true, out result))
                {
                    orderStateType = result;
                }
            }
            return orderStateType;
        }

        [HttpPost]
        [Route("api/driver/orders")]
        public IHttpActionResult Post(OrderDTO orderDto)
        {
            LogRequest();
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var order = mOrderService.GetOrder(orderDto.Id);
                if (order == null)
                {
                    mLogger.Debug("Order with {0} is not exists", orderDto.Id);
                    return DriverResult(HttpStatusCode.NotFound, ErrorFactory.OrderNotFound(orderDto.Id));
                }

                //Process change order state
                if (orderDto.State != null)
                {
                    var newOrderState = orderDto.State.Type;
                    var oldOrderState = order.State;

                    if (!driver.IsWorkAllowed() && newOrderState == OrderStateType.Assigned)//Only for a try to get new order
                    {
                        mLogger.Debug("Driver work is not allowed");
                        return DriverResult(HttpStatusCode.Forbidden, ErrorFactory.NotEnoughtMoney());
                    }

                    if (newOrderState == oldOrderState)
                    {
                        return Ok(orderDto);
                    }


                    if (newOrderState == OrderStateType.Assigned)
                    {
                        if (order.Driver != null)
                        {
                            return DriverResult(HttpStatusCode.Gone, ErrorFactory.OrderGivenAnotherDriver());
                        }
                        if (oldOrderState == OrderStateType.Created)
                        {
                            order.DriverId = driver.Id;
                            order.State = OrderStateType.Assigned;
                        }
                        else
                        {
                            return StatusCode(HttpStatusCode.Forbidden);
                        }
                    }
                    else
                    {
                        if (!(order.DriverId == driver.Id ||
                            order.DriverId == null ||
                            order.State == OrderStateType.Rejected))
                        {
                            return StatusCode(HttpStatusCode.Forbidden);
                        }
                        switch (oldOrderState)
                        {
                            case OrderStateType.Accepted:
                            case OrderStateType.Approved:
                                if (newOrderState == OrderStateType.Driving)
                                {
                                    order.State = OrderStateType.Driving;
                                }
                                if (newOrderState == OrderStateType.Rejected)
                                {
                                    order.State = OrderStateType.Rejected;
                                }
                                break;
                            case OrderStateType.Driving:
                                if (newOrderState == OrderStateType.Waiting)
                                {
                                    order.State = OrderStateType.Waiting;
                                }
                                break;
                            case OrderStateType.Waiting:
                                if (newOrderState == OrderStateType.Transporting)
                                {
                                    order.State = OrderStateType.Transporting;
                                }
                                break;
                            case OrderStateType.Transporting:
                                if (newOrderState == OrderStateType.Completed)
                                {
                                    order.State = OrderStateType.Completed;
                                }
                                break;
                            case OrderStateType.Assigned:
                                if (newOrderState == OrderStateType.Rejected)
                                {
                                    order.State = OrderStateType.Rejected;
                                }
                                if (newOrderState == OrderStateType.Accepted)
                                {
                                    order.State = OrderStateType.Accepted;
                                }
                                break;
                            case OrderStateType.Rejected:
                                if (newOrderState == OrderStateType.Accepted)
                                {
                                    order.State = OrderStateType.Accepted;
                                    order.DriverId = driver.Id;
                                }
                                break;
                            case OrderStateType.Created:
                                if (newOrderState == OrderStateType.Accepted)
                                {
                                    order.State = OrderStateType.Accepted;
                                    order.DriverId = driver.Id;
                                }
                                if (newOrderState == OrderStateType.Rejected)
                                {
                                    order.State = OrderStateType.Rejected;
                                    order.DriverId = driver.Id;
                                }
                                break;
                            default:
                                return StatusCode(HttpStatusCode.Forbidden);
                        }
                    }
                }

                if (orderDto.Cost > 0)
                {
                    order.Cost = orderDto.Cost;
                }

                order = mOrderService.UpdateOrder(order, "Водитель");

                var updatedOrderDto = ObjectMapper.Instance.Map<Order, OrderDTO>(order);
                updatedOrderDto.Driver = null;
                ProcessOrder(order, orderDto);
                return Ok(updatedOrderDto);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("api/driver/profile")]
        public IHttpActionResult Post(DriverDTO driverDto)
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                if (driver.State == DriverState.Fired)
                {
                    return DriverResult(HttpStatusCode.Forbidden, ErrorFactory.YouCantWork());
                }

                var existsDriver = mDriverService.GetDriver(driver.Id);
                var updatedDriver = existsDriver;

                if (driverDto.State != null)
                {
                    LogRequest();
                    var currentState = existsDriver.State;
                    var newState = driverDto.State.State;
                    if (currentState == DriverState.Busy)
                    {
                        return DriverResult(HttpStatusCode.Forbidden, ErrorFactory.OrderExecutionInProcess());
                    }
                    if (!(newState == DriverState.NotAvailable || newState == DriverState.ReadyToWork))
                    {
                        return StatusCode(HttpStatusCode.Forbidden);
                    }
                    updatedDriver.State = driverDto.State.State;
                    mLogger.Debug("Driver state {0} changed to {1}", updatedDriver.Id, updatedDriver.State);
                }

                if (driverDto.Location != null)
                {
                    updatedDriver.Location = ObjectMapper.Instance.Map<LocationDTO, Location>(driverDto.Location);
                    updatedDriver.LastTrackUpdateTime = DateTime.UtcNow;
                }

                if (driverDto.RobotSettings != null)
                {
                    updatedDriver.RobotSettings =
                        ObjectMapper.Instance.Map<RobotSettingsDTO, RobotSettings>(driverDto.RobotSettings);
                }

                updatedDriver = mDriverService.UpdateDriver(updatedDriver);
                var updatedDriverDto = ObjectMapper.Instance.Map<Driver, DriverDTO>(updatedDriver);
                updatedDriverDto.WorkConditions = null;
                return Ok(updatedDriverDto);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("api/driver/profile")]
        public IHttpActionResult GetDriver()
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var driverDto = ObjectMapper.Instance.Map<Driver, DriverDTO>(driver);
                driverDto.WorkConditions = null;
                return Ok(driverDto);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("api/driver/events")]
        public IHttpActionResult GetDriverEvents(DateTime timestamp)
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var events = mEventService.GetOrderStateChangedEvents(timestamp, driver.Id);
                var allEventDtos = ObjectMapper.Instance.Map<IList<EventOrderStateChanged>, List<EventOrderStateChangedDTO>>(events);

                foreach (var eventDto in allEventDtos)
                {
                    eventDto.Order.Driver = null;
                }

                return Ok(allEventDtos);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("api/driver/tariffs")]
        public List<TariffDTO> GetDriverTariffs()
        {
            LogRequest();
            var tariffs = mTariffsService.GetTariffs();
            var tariffDtos = ObjectMapper.Instance.Map<IList<Tariff>, List<TariffDTO>>(tariffs);
            tariffDtos.ForEach(t => t.RegionsCosts = null);
            return tariffDtos;
        }

        [HttpGet]
        [Route("api/driver/transactions")]
        public IHttpActionResult GetTransactions(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                if (!fromDate.HasValue)
                {
                    toDate = DateTime.UtcNow;
                    fromDate = toDate.Value.AddDays(-3).Date;
                }
                var transactions = mTransactionService.GetTransactions(driver.Id, null, null, null, fromDate.Value, toDate.Value);
                var transactionDtos = ObjectMapper.Instance.Map<IList<Transaction>, List<TransactionDTO>>(transactions);
                foreach (var transactionDto in transactionDtos)
                {
                    transactionDto.Driver = null;
                }
                return Ok(transactionDtos);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("api/driver/rating")]
        public IHttpActionResult GetDriverRating()
        {
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                var driverRating = new
                {
                    yandexRating = 4.0,
                    companyRating = 4.3,
                    averageScore = 4.74,
                    tracks = 70,
                    delayPerOrder = 0.3
                };
                return Ok(driverRating);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("api/driver/messages")]
        public IHttpActionResult AddMessage(string text)
        {
            LogRequest();
            var driver = UserAccount.Driver;
            if (driver != null)
            {
                //TODO
                return Ok();
            }
            return Unauthorized();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mEventService.Dispose();
                mOrderService.Dispose();
                mTariffsService.Dispose();
                mTransactionService.Dispose();
                mDriverService.Dispose();
            }

            base.Dispose(disposing);
        }

        //TODO: remove after web-728
        private void ProcessOrder(Order order, OrderDTO orderDTO)
        {
            orderDTO.Driver = null;
            if (orderDTO.Customer != null &&
                order.Customer != null &&
                !string.IsNullOrEmpty(order.Customer.Phone))
            {
                orderDTO.Customer.Phone = order.Customer.Phone;
            }
        }

        private void ProcessOrders(IList<Order> orders, IList<OrderDTO> orderDtos)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                ProcessOrder(orders[i], orderDtos[i]);
            }
        }
    }
}
