using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Factories;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;

namespace Dexpa.Core.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IOrderHistoryRepository mOrderHistoryRepository;

        public OrderHistoryService(IOrderHistoryRepository regionRepository)
        {
            mOrderHistoryRepository = regionRepository;
        }

        public IList<OrderHistory> GetOrderHistory(long orderId)
        {
            return mOrderHistoryRepository.List(o => o.OrderId == orderId);
        }

        public void Dispose()
        {
            mOrderHistoryRepository.Dispose();
        }
    }
}
