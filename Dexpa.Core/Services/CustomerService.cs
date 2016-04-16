using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository mRepository;
        private IOrderRepository mOrderRepository;
        private IOrganizationRepository mOrganizationRepository;

        public CustomerService(ICustomerRepository repository, IOrderRepository orderRepository, IOrganizationRepository organizationRepository)
        {
            mRepository = repository;
            mOrderRepository = orderRepository;
            mOrganizationRepository = organizationRepository;
        }

        public Customer GetCustomer(long customerId)
        {
            return mRepository.Single(c => c.Id == customerId);
        }

        public List<LightCustomer> GetCustomers(int skip, int take, string sortBy, SortOrder sortOrder = SortOrder.Asc)
        {
            System.Reflection.PropertyInfo prop = typeof(LightCustomer).GetProperty(sortBy);
            var customers = mRepository.GetCustomerOrdersCount();

            var customersList = customers.OrderBy(s => prop).Skip(skip).Take(take).ToList();

            return customersList;
        }

        public List<Customer> GetCustomers(long organizationId)
        {
            return mRepository.List(c => c.OrganizationId == organizationId).ToList();
        }

        public List<Customer> GetCustomers(string organizationCodeword)
        {
            var organization = mOrganizationRepository.Single(o => o.Codeword == organizationCodeword);
            if (organization != null)
            {
                return mRepository.List(c => c.OrganizationId == organization.Id).ToList();
            }
            return null;
        }

        public Customer AddCustomer(Customer customer)
        {
            customer = mRepository.Add(customer);
            mRepository.Commit();
            return customer;
        }

        public void DeleteCustomer(long customerId)
        {
            var driver = mRepository.Single(d => d.Id == customerId);
            if (driver != null)
            {
                mRepository.Delete(driver);
                mRepository.Commit();
            }
        }

        public IList<Customer> GetCustomers(string filterPhone, string name = null)
        {
            var filterPhone2 = filterPhone;
            if (filterPhone.StartsWith("+7"))
            {
                filterPhone2 = "8" + filterPhone.Substring(2);
            }
            else
            {
                if (filterPhone.StartsWith("8"))
                {
                    filterPhone2 = "+7" + filterPhone.Substring(1);
                }
            }
            return mRepository.List(c => (c.Phone == filterPhone || c.Phone == filterPhone2) &&
                (string.IsNullOrEmpty(name) || string.Equals(c.Name.ToLower(), name.ToLower())));
        }

        public IList<LightCustomer> GetSearchCustomers(string query)
        {
            List<LightCustomer> customers;
            List<LightCustomer> customersList = null;

            customers = mRepository.GetCustomerOrdersCount();

            customersList =
                customers
                    .Where(
                        c =>
                            (c.Name != null && c.Name.ToLower().StartsWith(query)) ||
                            c.Phone != null && c.Phone.EndsWith(query))
                    .ToList();

            return customersList;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            customer = mRepository.Update(customer);
            mRepository.Commit();
            return customer;
        }

        public int GetTotalCustomersCount()
        {
            return mRepository.Count();
        }

        public void Dispose()
        {
            mRepository.Dispose();
            mOrganizationRepository.Dispose();
            mOrderRepository.Dispose();
        }
    }
}
