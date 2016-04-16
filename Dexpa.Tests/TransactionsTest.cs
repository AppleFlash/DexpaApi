using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Dexpa.Tests
{
    [TestClass]
    public class TransactionsTest : ApiTestBase
    {
        [TestMethod]
        public void AddTransaction()
        {
            var transaction = GetTransaction();
            var content = JsonConvert.SerializeObject(transaction);
            ExecuteRequest("transaction", Method.Post, content);
        }

        private TransactionDTO GetTransaction()
        {
            var driversJson = ExecuteRequest("drivers", Method.Get);
            var driversDto = JsonConvert.DeserializeObject<List<DriverDTO>>(driversJson);
            var drivers = ObjectMapper.Instance.Map<List<DriverDTO>, List<Driver>>(driversDto);
            drivers[0].Id = driversDto[0].Id;//Need to fix mapper features

            var transaction = new Transaction
            {
                Type = TransactionType.Withdrawal,
                Amount = 100,
                Comment = "Для заказов",
                Driver = drivers[0],
                Group = TransactionGroup.Fine,
                PaymentMethod = PaymentMethod.Cash
            };

            var transactionDto = ObjectMapper.Instance.Map<Transaction, TransactionDTO>(transaction);
            return transactionDto;
        }

        [TestMethod]
        public void GetTransactions()
        {
            var today = DateTime.UtcNow;
            var query = string.Format("transaction/?fromdate={0}&todate={1}",
                Utils.FormatDateTime(today.Date),
                Utils.FormatDateTime(today.Date.AddDays(1)));
            var transactionsJson = ExecuteRequest(query, Method.Get);
            var transactions = JsonConvert.DeserializeObject<List<TransactionDTO>>(transactionsJson);
            var lastTransaction = ObjectMapper.Instance.Map<TransactionDTO, Transaction>(transactions.Last());
            var pattern = ObjectMapper.Instance.Map<TransactionDTO, Transaction>(GetTransaction());
            Assert.IsTrue(
                lastTransaction.Id > 0 &&
                lastTransaction.Driver != null &&
                lastTransaction.Comment == pattern.Comment &&
                lastTransaction.Driver.Id == pattern.Driver.Id &&
                lastTransaction.Group == pattern.Group &&
                lastTransaction.PaymentMethod == pattern.PaymentMethod &&
                lastTransaction.Type == pattern.Type &&
                Math.Abs(lastTransaction.Amount - pattern.Amount) < double.Epsilon
                );
        }
    }
}
