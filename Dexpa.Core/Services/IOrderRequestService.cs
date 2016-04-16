using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IOrderRequestService : IDisposable
    {
        OrderRequest AddRequest(OrderRequest orderRequest);

        IList<OrderRequest> GetOrderRequests(DateTime fromTimestamp);

        void RemoveRequests(IList<OrderRequest> requests);
    }
}