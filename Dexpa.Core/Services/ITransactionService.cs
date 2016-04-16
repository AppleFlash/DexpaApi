using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface ITransactionService : IDisposable
    {
        Transaction AddTransaction(Transaction transaction);

        IList<Transaction> GetTransactions(long? driverId, TransactionType? transactionType, PaymentMethod? paymentMethod, TransactionGroup? transactionGroup,
            DateTime fromDate, DateTime toDate);

        Transaction CreateCarRentTransaction(long driverId);

        Transaction CreateSupportTransaction(long driverId);
        
        double GetOrganizationBalance(long organizationId);

        void RecalculateDriverBalance();
    }
}