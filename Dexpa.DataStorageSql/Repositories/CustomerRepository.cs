using System.Data.Entity;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.DataStorageSql.Repositories
{
    public class CustomerRepository : ARepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context)
            : base(context)
        {
        }
    }
}
