using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class DriverOrderRequestService : IDriverOrderRequestService
    {
        private IDriverOrderRequestRepository mRequestRepository;

        private IOrderRepository mOrderRepository;

        private IDriverRepository mDriverRepository;

        public DriverOrderRequestService(IDriverOrderRequestRepository requestRepository,
            IOrderRepository orderRepository, IDriverRepository driverRepository)
        {
            mRequestRepository = requestRepository;
            mOrderRepository = orderRepository;
            mDriverRepository = driverRepository;
        }

        public IList<DriverOrderRequest> AddRequest(Order order, List<Driver> drivers)
        {
            var requests = new List<DriverOrderRequest>();
            foreach (var driver in drivers)
            {
                var request = new DriverOrderRequest
                {
                    Driver = driver,
                    DriverId = driver.Id,
                    Order = order,
                    OrderId = order.Id
                };
                mRequestRepository.Add(request);
                requests.Add(request);
            }

            mRequestRepository.Commit();

            return requests;
        }

        public DriverOrderRequest AddRequest(Order order, Driver driver, OrderRequestState state)
        {
            var request = new DriverOrderRequest
            {
                DriverId = driver.Id,
                OrderId = order.Id,
                State = state
            };
            UpdateRelationships(request);
            mRequestRepository.Add(request);

            mRequestRepository.Commit();

            return request;
        }

        public void UpdateRelationships(DriverOrderRequest request)
        {
            var driver = mDriverRepository.Single(d => d.Id == request.DriverId);
            request.Driver = driver;

            var order = mOrderRepository.Single(o => o.Id == request.OrderId);
            request.Order = order;
        }

        public IList<DriverOrderRequest> GetActualRequests(long driverId)
        {
            var requests = mRequestRepository.List(r => r.DriverId == driverId && r.State == OrderRequestState.New);
            return requests
                .Where(r => (r.Order.State == OrderStateType.Approved) &&
                            r.Order.DepartureDate >= DateTime.UtcNow)
                .ToList();
        }

        public DriverOrderRequest GetRequest(long driverId, long orderId)
        {
            return mRequestRepository.Single(r => r.DriverId == driverId && r.OrderId == orderId);
        }

        public IList<DriverOrderRequest> GetRequests()
        {
            return mRequestRepository.List();
        }

        public IList<DriverOrderRequest> GetRequests(long orderId)
        {
            return mRequestRepository.List(r => r.OrderId == orderId);
        }

        public void RemoveObsoletedRequests()
        {
            var nowTime = DateTime.UtcNow;

            var requests = mRequestRepository.List();
            var requestsToRemove = new List<DriverOrderRequest>();
            for (int i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                var orderState = request.Order.State;
                if (request.State == OrderRequestState.Rejected ||
                    request.Order.DepartureDate <= nowTime ||
                    orderState == OrderStateType.Accepted ||
                    orderState == OrderStateType.Approved ||
                    orderState == OrderStateType.Canceled ||
                    orderState == OrderStateType.Disapproved ||
                    orderState == OrderStateType.Driving ||
                    orderState == OrderStateType.Transporting ||
                    orderState == OrderStateType.Waiting)
                {
                    requestsToRemove.Add(request);
                }
            }

            foreach (var request in requestsToRemove)
            {
                mRequestRepository.Delete(request);
            }

            mRequestRepository.Commit();
        }

        public void RemoveRequests(long orderId)
        {
            var requests = mRequestRepository.List(r => r.OrderId == orderId);
            foreach (var request in requests)
            {
                mRequestRepository.Delete(request);
            }
            mRequestRepository.Commit();
        }

        public void RemoveRequests(IList<DriverOrderRequest> requests)
        {
            foreach (var request in requests)
            {
                mRequestRepository.Delete(request);
            }

            mRequestRepository.Commit();
        }

        public IList<DriverOrderRequest> GetRequests(OrderRequestState state)
        {
            return mRequestRepository.List(r => r.State == state);
        }

        public void UpdateRequest(DriverOrderRequest orderRequest)
        {
            mRequestRepository.Update(orderRequest);
            mRequestRepository.Commit();
        }

        public void Dispose()
        {
            mRequestRepository.Dispose();
        }
    }
}
