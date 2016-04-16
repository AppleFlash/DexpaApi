using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class OrderRequestService : IOrderRequestService
    {
        private IOrderRequestRepository mRequestRepository;

        public OrderRequestService(IOrderRequestRepository orderRequestRepository)
        {
            mRequestRepository = orderRequestRepository;
        }

        public OrderRequest AddRequest(OrderRequest orderRequest)
        {
            mRequestRepository.Add(orderRequest);
            mRequestRepository.Commit();
            return orderRequest;
        }

        public IList<OrderRequest> GetOrderRequests(DateTime fromTimestamp)
        {
            return mRequestRepository.List(r => r.Timestamp > fromTimestamp);
        }

        public void RemoveRequests(IList<OrderRequest> requests)
        {
            foreach (var request in requests)
            {
                mRequestRepository.Delete(request);
            }

            mRequestRepository.Commit();
        }

        public void Dispose()
        {
            mRequestRepository.Dispose();
        }
    }
}
