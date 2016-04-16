using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class CustomersController : ApiControllerBase
    {
        private ICustomerService mCustomerService;

        public CustomersController(ICustomerService customerService)
        {
            mCustomerService = customerService;
        }

        public IEnumerable<LightCustomer> GetCustomers(int skip = 0, int take = 50, string sortBy = "Id", SortOrder orderBy = SortOrder.Asc)
        {
            var customers = mCustomerService.GetCustomers(skip, take, sortBy, orderBy);
            //return ObjectMapper.Instance.Map<IList<CustomerReportItem>, List<CustomerReportItemDTO>>(customers);
            return customers;
        }

        public IEnumerable<CustomerDTO> GetCustomers(string filterPhone, int skip = 0, int take = 50)
        {
            var customers = mCustomerService.GetCustomers(filterPhone, null).Skip(skip).Take(take).ToList();
            return ObjectMapper.Instance.Map<IList<Customer>, List<CustomerDTO>>(customers);
        }

        [ValidateModel]
        public HttpResponseMessage Post(CustomerDTO customer)
        {
            var customerModel = ObjectMapper.Instance.Map<CustomerDTO, Customer>(customer);
            var addCustomer = mCustomerService.AddCustomer(customerModel);
            return Request.CreateResponse(ObjectMapper.Instance.Map<Customer, CustomerDTO>(addCustomer));
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, CustomerDTO customer)
        {
            var existsCutomer = mCustomerService.GetCustomer(id);
            if (existsCutomer == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var updatedCustomer = ObjectMapper.Instance.Map(customer, existsCutomer);
            mCustomerService.UpdateCustomer(updatedCustomer);
            return Ok(ObjectMapper.Instance.Map<Customer, CustomerDTO>(updatedCustomer));
        }

        public void Delete(int id)
        {
            mCustomerService.DeleteCustomer(id);
        }

        public IEnumerable<LightCustomer> GetCustomersSearch(string query, int skip = 0, int take = 50)
        {
            var customers = mCustomerService.GetSearchCustomers(query.ToLower());
            var customersPaged = customers.Skip(skip).Take(take).ToList(); //запись количества всех результатов в виде последнего клиента
            customersPaged.Add(new LightCustomer()
            {
                Phone = customers.Count().ToString()
            });
            //return ObjectMapper.Instance.Map<IList<CustomerReportItem>, List<CustomerReportItemDTO>>(customersPaged);
            return customersPaged;
        }

        public IEnumerable<CustomerDTO> GetCustomers(long organizationId)
        {
            var customers = mCustomerService.GetCustomers(organizationId);
            return ObjectMapper.Instance.Map<IList<Customer>, List<CustomerDTO>>(customers);
        }

        public IEnumerable<CustomerDTO> GetCustomers(string organizationCodeword)
        {
            var customers = mCustomerService.GetCustomers(organizationCodeword);
            return ObjectMapper.Instance.Map<IList<Customer>, List<CustomerDTO>>(customers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mCustomerService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
