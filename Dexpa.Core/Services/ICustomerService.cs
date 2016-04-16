using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Services
{
    public interface ICustomerService : IDisposable
    {
        Customer GetCustomer(long customerId);

        Customer AddCustomer(Customer customer);

        void DeleteCustomer(long customerId);

        IList<Customer> GetCustomers(string filterPhone, string name = null);

        IList<LightCustomer> GetSearchCustomers(string query);

        Customer UpdateCustomer(Customer customer);

        int GetTotalCustomersCount();

        List<LightCustomer> GetCustomers(int skip, int take, string sortBy, SortOrder sortOrder);

        List<Customer> GetCustomers(long organizationId);

        List<Customer> GetCustomers(string organizationCodeword);
    }
}
