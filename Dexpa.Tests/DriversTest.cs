using System.Collections.Generic;
using System.Linq;
using Dexpa.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dexpa.Tests
{
    [TestClass]
    public class DriversTest : ApiTestBase
    {
        [TestMethod]
        public void GetAllDrivers()
        {
            ExecuteRequest("drivers", Method.Get);
        }

        [TestMethod]
        public void AddDriver()
        {
            var driver = new DriverDTO()
            {
                FirstName = "TestDriverFirst2",
                LastName = "TestDriverLast2",
                MiddleName = "TestDriverMiddle2",
                Phones = new List<long>(new long[] { 777777 })
            };
            var content = JsonConvert.SerializeObject(driver);
            ExecuteRequest("drivers", Method.Post, content);
        }

        [TestMethod]
        public void UpdateDriver()
        {
            var driversJson = ExecuteRequest("drivers", Method.Get);
            var drivers = JsonConvert.DeserializeObject<List<DriverDTO>>(driversJson);
            var existsDriver = drivers.FirstOrDefault(d => d.FirstName == "TestDriverFirst2");

            var driver = new DriverDTO()
            {
                FirstName = "TestDriverFirstNew",
            };
            var content = JsonConvert.SerializeObject(driver);
            ExecuteRequest("drivers", Method.Put, existsDriver.Id, content);
        }


        [TestMethod]
        public void DeleteDriver()
        {
            var driversJson = ExecuteRequest("drivers", Method.Get);
            var drivers = JsonConvert.DeserializeObject<List<DriverDTO>>(driversJson);
            var existsDriver = drivers.FirstOrDefault(d => d.FirstName == "TestDriverFirstNew");

            ExecuteRequest("drivers", Method.Delete, existsDriver.Id);
        }
    }
}
