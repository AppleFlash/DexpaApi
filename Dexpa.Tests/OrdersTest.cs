using System;
using System.Collections.Generic;
using Dexpa.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dexpa.Tests
{
    [TestClass]
    public class OrdersTest : ApiTestBase
    {
        [TestMethodAttribute]
        public void GetOrders()
        {
            ExecuteRequest("orders/?date=01-03-2014", Method.Get);
        }


        [TestMethodAttribute]
        public void AddOrderNewCustomer()
        {
            //var customers = ExecuteRequest("customers", Method.Get);
            var driversJson = ExecuteRequest("drivers", Method.Get);
            var drivers = JsonConvert.DeserializeObject<List<DriverDTO>>(driversJson);

            var customer = new CustomerDTO()
            {
                Name = "TestCutomer",
                Phone = "123123"
            };

            var order = new OrderDTO()
            {
                Cost = 1000,
                Customer = customer,
                Driver = drivers[0],
                DepartureDate = new DateTime(2014,1,1,5,10,0),
                //FromAddress = "Test from address",
                //ToAddress = "Test to address"
            };

            var content = JsonConvert.SerializeObject(order);
            ExecuteRequest("orders", Method.Post, content);
        }

        [TestMethodAttribute]
        public void UpdateOrderState()
        {
            var orders = ExecuteRequest("orders", Method.Get);
            var driver = new DriverDTO()
            {
                FirstName = "TestDriverFirstNew",
                LastName = "TestDriverLast",
                MiddleName = "TestDriverMiddle",
                Phones = new List<long>(new long[] { 888888 })
            };
            var content = JsonConvert.SerializeObject(driver);
            ExecuteRequest("orders", Method.Put, 1, content);
        }
    }
}
