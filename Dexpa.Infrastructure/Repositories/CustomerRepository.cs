using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class CustomerRepository : ARepository<Customer>, ICustomerRepository
    {
        protected DbSet<Order> mOrdersSet;
        protected DbSet<Customer> mCustomersSet;

        public CustomerRepository(DbContext context)
            : base(context)
        {
            mOrdersSet = mContext.Set<Order>();
            mCustomersSet = mContext.Set<Customer>();
        }

        public List<LightCustomer> GetCustomerOrdersCount()
        {
            var customesCounts =
                mCustomersSet.GroupJoin(mOrdersSet.Where(or => or.Source != OrderSource.Yandex), c => c.Id,
                    o => o.CustomerId, (c, o) => new LightCustomer()
                    {
                        Id = c.Id,
                        Orders = o.Count(),
                        Name = c.Name,
                        Phone = c.Phone
                    });

            return customesCounts.ToList();
        }
    }
}
