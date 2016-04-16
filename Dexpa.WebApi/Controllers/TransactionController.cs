using System;
using System.Collections.Generic;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class TransactionController : ApiControllerBase
    {
        private ITransactionService mTransactionService;

        public TransactionController(ITransactionService transactionService)
        {
            mTransactionService = transactionService;
        }

        public TransactionDTO Post(TransactionDTO transactionDto)
        {
            var transaction = ObjectMapper.Instance.Map<TransactionDTO, Transaction>(transactionDto);
            var result = mTransactionService.AddTransaction(transaction);
            return ObjectMapper.Instance.Map<Transaction, TransactionDTO>(result);
        }

        public IList<TransactionDTO> Get(DateTime fromDate, DateTime toDate)
        {
            var fromUtc = TimeConverter.LocalToUtc(fromDate);
            var toUtc = TimeConverter.LocalToUtc(toDate);
            var transactions = mTransactionService.GetTransactions(null, null, null, null, fromUtc, toUtc);
            return ObjectMapper.Instance.Map<IList<Transaction>, List<TransactionDTO>>(transactions);
        }

        [HttpGet]
        [Route("api/Transaction/Driver")]
        public IList<TransactionDTO> GetDriverTransations(long driverId, DateTime fromDate, DateTime toDate)
        {
            var fromUtc = TimeConverter.LocalToUtc(fromDate);
            var toUtc = TimeConverter.LocalToUtc(toDate);
            var transactions = mTransactionService.GetTransactions(driverId, null, null, null, fromUtc, toUtc);
            return ObjectMapper.Instance.Map<IList<Transaction>, List<TransactionDTO>>(transactions);
        }

        public double GetOrganizationsBalance(long organizationId)
        {
            return mTransactionService.GetOrganizationBalance(organizationId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mTransactionService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
