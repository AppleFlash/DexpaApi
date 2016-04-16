using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Additional;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Services
{
    public interface IOrderService : IDisposable
    {
        Order GetOrder(long orderId);

        Order AddOrder(Order order, Customer customer = null);

        void DeleteOrder(long orderId);

        IList<Order> GetOrders(DateTime date);

        Order UpdateOrder(Order order, string updateReason, Customer customer = null);

        IList<Order> GetOrders(DateTime fromDate, DateTime toDate, long? driverId = null, OrderStateType? orderStateType = null);

        IList<Order> GetOrders(DateTime fromDate, DateTime toDate, OrderStateType? orderStateType = null);

        int GetTotalOrdersCount();

        IList<Order> GetOrders();

        IList<OrderWithPriority> GetActiveOrders();

        IList<Order> GetUnassignedOrders();

        IList<Order> GetOrders(long customerId, int take);

        OrderHistory GetLastStateTime(long orderId, OrderStateType orderState);

        Order GetOrder(string sourceOrderId);

        Order GetLastOrder(long driverId);
        
        Order GetDriverCurrentOrder(long driverId);

        Order GetCustomerCurrentOrder(long customerId);

        List<LightOrderWithPriority> GetLightOrders();

        List<LightOrderWithPriority> GetLightOrders(IEnumerable<OrderWithPriority> orders);

        double CalculateOrderCost(OrderPathWithTariff orderPath);

        void AssigneDrivers(long orderId, List<long> driversIds);

        void UnnasigneDrivers(long orderId, List<long> unassignedDrivers);

        List<long> GetUnassignedDrivers();

        List<Order> GetFeedbackOrders(List<string> sourceOrderIds);
    }
}
