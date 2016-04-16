using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IOrderHistoryService : IDisposable
    {
        IList<OrderHistory> GetOrderHistory(long orderId);
    }
}
