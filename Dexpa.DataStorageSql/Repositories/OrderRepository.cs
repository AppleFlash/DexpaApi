using System;
using System.Collections.Generic;
using System.Data.Entity;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.DataStorageSql.Repositories
{
    public class OrderRepository : ARepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context)
            : base(context)
        {
        }

        public IOrder CreateOrder(ICustomer customer, string fromAddress, string toAddress, DateTime departureDate)
        {
            throw new NotImplementedException();
        }

        public IList<IOrder> GetOrders(DateTime date)
        {
            throw new NotImplementedException();
        }

        public IList<IOrder> GetOrders(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(IOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
