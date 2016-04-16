using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class CustomerAddressesService : ICustomerAddressesService
    {
        private ICustomerAddressesRepository mRepository;

        public CustomerAddressesService(ICustomerAddressesRepository customerAddressesRepository)
        {
            mRepository = customerAddressesRepository;
        }


        public IList<CustomerAddresses> GetCustomerAddresses()
        {
            return mRepository.List();
        }

        public IList<CustomerAddresses> GetCustomerAddresses(long customerId)
        {
            return mRepository.List(c => c.CustomerId == customerId);
        }

        public CustomerAddresses AddCustomerAddresses(CustomerAddresses customerAddresses)
        {
            customerAddresses = mRepository.Add(customerAddresses);
            mRepository.Commit();
            return customerAddresses;
        }

        public void DeleteCustomerAddresses(long id)
        {
            CustomerAddresses customerAddresses = mRepository.Single(c => c.Id == id);
            if (customerAddresses != null)
            {
                mRepository.Delete(customerAddresses);
                mRepository.Commit();
            }
        }

        public void Dispose()
        {
            mRepository.Dispose();
        }
    }
}
