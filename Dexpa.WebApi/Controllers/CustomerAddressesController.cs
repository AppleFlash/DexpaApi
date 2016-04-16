using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;
using Newtonsoft.Json.Bson;

namespace Dexpa.WebApi.Controllers
{
    public class CustomerAddressesController : ApiControllerBase
    {
        private ICustomerAddressesService mService;

        public CustomerAddressesController(ICustomerAddressesService service)
        {
            mService = service;
        }

        public IEnumerable<CustomerAddressesDTO> GetCustomerAddresses()
        {
            return ObjectMapper.Instance.Map<IList<CustomerAddresses>, List<CustomerAddressesDTO>>(mService.GetCustomerAddresses());
        }

        public IEnumerable<CustomerAddressesDTO> GetCustomerAddresses(long customerId)
        {
            return ObjectMapper.Instance.Map<IList<CustomerAddresses>, List<CustomerAddressesDTO>>(mService.GetCustomerAddresses(customerId));
        }

        public IHttpActionResult Post(CustomerAddressesDTO customerAddressesDTO)
        {
            var customerAddress = new CustomerAddresses();
            ObjectMapper.Instance.Map(customerAddressesDTO, customerAddress);
            var addCustomerAddress = mService.AddCustomerAddresses(customerAddress);
            if (addCustomerAddress != null)
            {
                return Ok(ObjectMapper.Instance.Map<CustomerAddresses, CustomerAddressesDTO>(addCustomerAddress));
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            mService.DeleteCustomerAddresses(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}