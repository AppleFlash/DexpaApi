using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.DTO;
using Dexpa.DTO.HelpDictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dexpa.Tests
{
    [TestClass]
    public class DriverWorkConditionsTest : ApiTestBase
    {
        [TestMethod]
        public void GetAllConditions()
        {
            ExecuteRequest("DriverWorkConditions", Method.Get);
        }

        [TestMethod]
        public void AddNewWorkCondition()
        {
            var orderTypesJson = ExecuteRequest("helpdictionaries/OrderTypes", Method.Get);
            var orderTypesDto = JsonConvert.DeserializeObject<List<OrderTypeDTO>>(orderTypesJson);

            var orderFees = new List<OrderFeeDTO>();
            for (int i = 0; i < 2; i++)
            {

                foreach (var orderTypeDto in orderTypesDto)
                {
                    var orderFee = new OrderFeeDTO
                    {
                        FeeType = OrderFeeType.Percent,
                        OrderType = orderTypeDto,
                        Value = 50
                    };

                    orderFees.Add(orderFee);
                }
            }

            var conditions = new DriverWorkConditionsDTO()
            {
                Name = "Test",
                OrderFees = orderFees
            };

            var content = JsonConvert.SerializeObject(conditions);
            ExecuteRequest("DriverWorkConditions", Method.Post, content);
        }

        [TestMethod]
        public void UpdateWorkConditions()
        {
            var conditions = GetConditions("Test");

            conditions.Name = "NewTest";
            conditions.OrderFees.Remove(conditions.OrderFees.Last());
            conditions.OrderFees.First().Value = 70;

            var content = JsonConvert.SerializeObject(conditions);
            ExecuteRequest("DriverWorkConditions", Method.Put, conditions.Id, content);

            var updatedConditions = GetConditions("NewTest");
            Assert.AreEqual(conditions.OrderFees.Count, updatedConditions.OrderFees.Count);
            foreach (var orderFee in conditions.OrderFees)
            {
                var updatedOrderFee = updatedConditions.OrderFees.FirstOrDefault(of => of.Id == orderFee.Id);
                Assert.IsNotNull(updatedOrderFee);
                Assert.AreEqual(orderFee.FeeType, updatedOrderFee.FeeType);
                Assert.IsTrue(Math.Abs(orderFee.Value - updatedOrderFee.Value) < double.Epsilon);
            }
        }

        private DriverWorkConditionsDTO GetConditions(string name)
        {
            var conditionsJson = ExecuteRequest("DriverWorkConditions", Method.Get);
            var conditionsList = JsonConvert.DeserializeObject<List<DriverWorkConditionsDTO>>(conditionsJson);
            var conditions = conditionsList.FirstOrDefault(d => d.Name == name);
            return conditions;
        }


        [TestMethod]
        public void DeleteConditions()
        {
            var conditions = GetConditions("NewTest");

            ExecuteRequest("DriverWorkConditions", Method.Delete, conditions.Id);
        }
    }
}
