using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Repositories
{
    public interface ICustomerRepository : ICRUDRepository<Customer>
    {
        List<LightCustomer> GetCustomerOrdersCount();
    }
}
