using System.Linq;
using System.Linq.Dynamic;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Yandex.Synchronizer.Helpers;
using Yandex.Taxi.Model.Blacklist;
using Yandex.Taxi.Model.Drivers;
using Yandex.Taxi.Model.Orders;
using Order = Yandex.Taxi.Model.Orders.Order;

namespace Yandex.Synchronizer.Controllers
{
    [RoutePrefix("api/taxi")]
    public class YandexController : ApiController
    {
        private Logger mLogger = LogManager.GetCurrentClassLogger();
        private IDriverService mDriverService;
        private IOrderService mOrderService;
        private ICustomerService mCustomerService;
        private IOrderRequestService mOrderRequestService;
        public YandexController()
        {
            this.mDriverService = NinjectWebCommon.Resolve<IDriverService>();
            this.mOrderService = NinjectWebCommon.Resolve<IOrderService>();
            this.mCustomerService = NinjectWebCommon.Resolve<ICustomerService>();
            this.mOrderRequestService = NinjectWebCommon.Resolve<IOrderRequestService>();
        }

        [HttpGet, Route("drivers")]
        public Cars GetDrivers()
        {
            mLogger.Debug("Get Drivers");
            Cars result;
            try
            {
                var drivers = this.mDriverService.GetDrivers(false);
                result = DataMapHelper.MapDrivers(drivers);
            }
            catch (Exception ex)
            {
                this.mLogger.Error("GetDrivers", ex);
                throw ex;
            }
            return result;
        }

        [HttpGet, Route("blacklist")]
        public Blacklist GetBlacklist()
        {
            Blacklist blacklist = new Blacklist();
            return blacklist;
        }

        [HttpPost, Route("1.x/requestcar")]
        public IHttpActionResult NewOrder(Order order)
        {
            mLogger.Debug("New Order {0}, {1}", order.Id, order.Source.FullName);
            try
            {
                var orderRequest = new OrderRequest
                {
                    OrderUid = order.Id,
                    DataJson = JsonConvert.SerializeObject(order),
                    Type = OrderRequestType.New
                };
                mOrderRequestService.AddRequest(orderRequest);
            }
            catch (Exception ex)
            {
                mLogger.Error("NewOrder", ex);
            }
            return Ok();
        }

        [HttpPost, Route("1.x/setcar")]
        public IHttpActionResult CommitOrder(OrderConfirmation orderConfirmation)
        {
            this.mLogger.Debug("Commit Order {0}", orderConfirmation.Id);
            try
            {
                Dexpa.Core.Model.Order order = mOrderService.GetOrder(orderConfirmation.Id);
                if (order == null)
                {
                    mLogger.Error("Can't commit order. The order {0} is not exists", orderConfirmation.Id);
                    return BadRequest();
                }
                if (order.OrderHistories.Any(a => a.OrderState == OrderStateType.Approved))
                {
                    mLogger.Debug("Order already approved {0}. Drivers selected: {1}",
                        orderConfirmation.Id,
                        orderConfirmation.Cars.Count);
                    return Ok();
                }

                var cars = orderConfirmation.Cars;
                Driver driver = null;
                if (cars.Count > 0)
                {
                    driver = mDriverService.GetDriver(long.Parse(cars[0].Uuid));
                    if (driver == null || order.State != OrderStateType.Created)
                    {
                        mLogger.Debug("Order {0} already assigned.", orderConfirmation.Id);
                        return StatusCode(HttpStatusCode.Gone);
                    }
                }
                var contactInfo = orderConfirmation.ContactInfo;
                Customer customer = null;
                foreach (string current in contactInfo.Phones)
                {
                    var customers = mCustomerService.GetCustomers(current, contactInfo.Name);
                    if (customers.Count > 0)
                    {
                        customer = customers[0];
                        break;
                    }
                }
                if (customer == null)
                {
                    customer = new Customer();
                    customer.Name = contactInfo.Name;
                    customer.Phone = contactInfo.Phones[0];
                    if (contactInfo.Phones.Count > 1)
                    {
                        customer.PrivatePhone = contactInfo.Phones[1];
                    }
                    customer = mCustomerService.AddCustomer(customer);

                    foreach (var phone in contactInfo.Phones)
                    {
                        mLogger.Debug("Customer phone: {0}", phone);
                    }
                }
                order.CustomerId = customer.Id;
                order.State = OrderStateType.Approved;
                if (driver != null)
                {
                    order.DriverId = driver.Id;
                }

                mOrderService.UpdateOrder(order, "Yandex подтвердил заказ");
            }
            catch (Exception ex)
            {
                mLogger.Error("CommitOrder", ex);
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet, Route("1.x/cancelrequest")]
        public IHttpActionResult CancelOrder(string orderId, CancelReason reason, string duplicate = null)
        {
            mLogger.Debug("Cancel Order {0}. Reason: {1}", orderId, reason);
            try
            {
                var order = mOrderService.GetOrder(orderId);
                if (order != null)
                {
                    if (order.State == OrderStateType.Completed)
                    {
                        mLogger.Debug("Current order state {0} is not consistent {1}.", order.State, orderId);
                        return BadRequest();
                    }
                }
                var orderRequest = new OrderRequest
                {
                    OrderUid = orderId,
                    DataJson = reason.ToString(),
                    Type = OrderRequestType.Cancelled
                };
                mOrderRequestService.AddRequest(orderRequest);
            }
            catch (Exception ex)
            {
                mLogger.Error("CancelOrder", ex);
            }
            return Ok();
        }
    }
}
