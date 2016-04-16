using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface ICustomerAddressesService : IDisposable
    {
        IList<CustomerAddresses> GetCustomerAddresses();

        IList<CustomerAddresses> GetCustomerAddresses(long customerId);

        CustomerAddresses AddCustomerAddresses(CustomerAddresses customerAddresses);

        void DeleteCustomerAddresses(long id);
    }
}
