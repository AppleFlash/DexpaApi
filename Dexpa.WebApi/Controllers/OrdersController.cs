using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Additional;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;
using Microsoft.Ajax.Utilities;

namespace Dexpa.WebApi.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        private IOrderService mOrderService;

        private IOrderHistoryService mOrderHistoryService;

        private const int OrderAddressesResultCount = 3;

        public OrdersController(IOrderService orderService, IOrderHistoryService orderHistoryService)
        {
            mOrderService = orderService;
            mOrderHistoryService = orderHistoryService;
        }

        public IEnumerable<OrderDTO> GetOrders(DateTime date)
        {
            var source = mOrderService.GetOrders(date);
            var orders = ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(source);
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].AcceptTime = SetLastStateTime(source[i].Id, OrderStateType.Accepted);
                orders[i].StartWaitTime = SetLastStateTime(source[i].Id, OrderStateType.Waiting);
            }

            return orders;
        }

        [Route("api/Orders/Light")]
        public IEnumerable<LightOrderWithPriority> GetLightOrders()
        {
            var orders = mOrderService.GetLightOrders();
            foreach (var order in orders)
            {
                order.Order.StartWaitTime = SetLastStateTime(order.Order.Id, OrderStateType.Waiting);
            }

            return orders;
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            var source = mOrderService.GetOrders();
            var orders = ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(source);
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].AcceptTime = SetLastStateTime(source[i].Id, OrderStateType.Accepted);
                orders[i].StartWaitTime = SetLastStateTime(source[i].Id, OrderStateType.Waiting);
            }

            return orders;
        }

        [Route("api/Orders/activeOrders")]
        [HttpGet]
        public IEnumerable<OrderWithPriorityDTO> ActiveOrders()
        {
            var source = mOrderService.GetActiveOrders();
            var orders = ObjectMapper.Instance.Map<IList<OrderWithPriority>, List<OrderWithPriorityDTO>>(source);
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].Order.AcceptTime = SetLastStateTime(source[i].Order.Id, OrderStateType.Accepted);
                orders[i].Order.StartWaitTime = SetLastStateTime(source[i].Order.Id, OrderStateType.Waiting);
            }

            return orders;
        }

        [Route("api/Orders/{id}/history")]
        [HttpGet]
        public IList<OrderHistoryDTO> OrderHistory(long id)
        {
            var source = mOrderHistoryService.GetOrderHistory(id);
            var history = ObjectMapper.Instance.Map<IList<OrderHistory>, List<OrderHistoryDTO>>(source);

            return history;
        }

        [Route("api/Orders/GetDriverCurrentOrder")]
        public OrderDTO GetDriverCurrentOrder(long driverId)
        {
            return ObjectMapper.Instance.Map<Order, OrderDTO>(mOrderService.GetDriverCurrentOrder(driverId));
        }

        [Route("api/Orders/GetCustomerCurrentOrder")]
        public OrderDTO GetCustomerCurrentOrder(long customerId)
        {
            return ObjectMapper.Instance.Map<Order, OrderDTO>(mOrderService.GetCustomerCurrentOrder(customerId));
        }

        public OrderDTO GetOrder(string sourceOrderId)
        {
            var source = mOrderService.GetOrder(sourceOrderId);
            var orders = ObjectMapper.Instance.Map<Order, OrderDTO>(source);

            return orders;
        }

        private OrderHistory GetLastStateTime(long orderId, OrderStateType orderState)
        {
            return mOrderService.GetLastStateTime(orderId, orderState);
        }

        private DateTime? SetLastStateTime(long orderId, OrderStateType orderState)
        {
            OrderHistory orderHistory = GetLastStateTime(orderId, orderState);
            if (orderHistory != null)
            {
                return TimeConverter.UtcToLocal(orderHistory.Timestamp);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<OrderDTO> GetOrders(long customerId)
        {
            var source = mOrderService.GetOrders(customerId, OrderAddressesResultCount);
            var orders = ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(source);

            return orders;
        }

        public IHttpActionResult GetOrder(long id)
        {
            var order = mOrderService.GetOrder(id);
            if (order != null)
            {
                return Ok(ObjectMapper.Instance.Map<Order, OrderDTO>(order));
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        public IEnumerable<OrderDTO> GetOrders(DateTime fromDate, DateTime toDate)
        {
            return ObjectMapper.Instance.Map<IList<Order>, List<OrderDTO>>(mOrderService.GetOrders(fromDate, toDate, null));
        }

        [ValidateModel]
        public IHttpActionResult Post(CreateOrderDTO orderDTO)
        {
            if (orderDTO.OrderOptions == null)
            {
                orderDTO.OrderOptions = new OrderOptionsDTO();
            }

            Customer customer = null;
            if (orderDTO.Customer != null)
            {
                customer = ObjectMapper.Instance.Map<CustomerDTO, Customer>(orderDTO.Customer);
            }

            if (orderDTO.Drivers == null)
            {
                orderDTO.Driver = null;
                var orderModel = ObjectMapper.Instance.Map<OrderDTO, Order>(orderDTO);
                mOrderService.AddOrder(orderModel, customer);
            }
            else
            {
                foreach (var driver in orderDTO.Drivers)
                {
                    orderDTO.Driver = driver;
                    var orderModel = ObjectMapper.Instance.Map<OrderDTO, Order>(orderDTO);
                    mOrderService.AddOrder(orderModel, customer);
                }
            }


            return Ok();
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, UpdateOrderDTO updateOrderDTO)
        {
            updateOrderDTO.Order.Driver.Car = null;
            var existsOrder = mOrderService.GetOrder(id);
            if (existsOrder == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            Customer customer = null;
            if (updateOrderDTO.Order.Customer != null)
            {
                customer = ObjectMapper.Instance.Map<CustomerDTO, Customer>(updateOrderDTO.Order.Customer);
            }

            var updatedOrder = ObjectMapper.Instance.Map(updateOrderDTO.Order, existsOrder);
            mOrderService.UpdateOrder(updatedOrder, updateOrderDTO.UpdateCancelReason, customer);
            return Ok(ObjectMapper.Instance.Map<Order, OrderDTO>(updatedOrder));
        }

        [Route("api/Orders/{id}/State")]
        [HttpPost]
        [ValidateModel]
        public IHttpActionResult UpdateOrderState(long id, UpdateLightOrderDTO updateOrderDTO)
        {
            var existsOrder = mOrderService.GetOrder(id);
            if (existsOrder == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            existsOrder.State = updateOrderDTO.Order.State;

            var updatedOrder = mOrderService.UpdateOrder(existsOrder, updateOrderDTO.UpdateCancelReason);
            return Ok(ObjectMapper.Instance.Map<Order, OrderDTO>(updatedOrder));
        }

        [Route("api/Orders/test")]
        [HttpGet]
        public void ResetOrder()
        {
            //Для теста. Удалить метод после того, как закроется web-811
            var order = mOrderService.GetOrder(10019);
            order.DepartureDate = DateTime.UtcNow.AddMinutes(25);
            order.State = OrderStateType.Approved;
            mOrderService.UpdateOrder(order, "Test web-811");
        }

        [Route("api/Orders/Cost")]
        [HttpPost]
        public IHttpActionResult GetCost(OrderPathWithTariff orderPath)
        {
            var cost =  mOrderService.CalculateOrderCost(orderPath);
            return Ok(cost);
        }


        public void Delete(int id)
        {
            mOrderService.DeleteOrder(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mOrderHistoryService.Dispose();
                mOrderService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}