using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;


namespace Dexpa.Infrastructure.Repositories
{
    public class CustomerAddressesRepository:ARepository<CustomerAddresses>,ICustomerAddressesRepository
    {
        public CustomerAddressesRepository(DbContext context) : base(context)
        {
        }
    }
}
