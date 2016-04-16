using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IDriverOrderRequestService : IDisposable
    {
        IList<DriverOrderRequest> AddRequest(Order order, List<Driver> drivers);

        DriverOrderRequest AddRequest(Order order, Driver driver, OrderRequestState state);
        IList<DriverOrderRequest> GetActualRequests(long driverId);
        DriverOrderRequest GetRequest(long driverId, long orderId);
        IList<DriverOrderRequest> GetRequests();
        IList<DriverOrderRequest> GetRequests(long orderId);
        void RemoveObsoletedRequests();
        void UpdateRequest(DriverOrderRequest orderRequest);
        void RemoveRequests(long orderId);
        IList<DriverOrderRequest> GetRequests(OrderRequestState orderId);
        void RemoveRequests(IList<DriverOrderRequest> orderId);
    }
}