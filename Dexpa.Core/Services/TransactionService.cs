using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository mTransactionRepository;

        private IDriverRepository mDriverRepository;

        private IOrderRepository mOrderRepository;

        private IGlobalSettingsRepository mGlobalSettingsRepository;

        public TransactionService(ITransactionRepository transactionRepository, IDriverRepository driverRepository, IOrderRepository orderRepository, IGlobalSettingsRepository globalSettingsRepository)
        {
            mTransactionRepository = transactionRepository;
            mDriverRepository = driverRepository;
            mOrderRepository = orderRepository;
            mGlobalSettingsRepository = globalSettingsRepository;
        }

        public Transaction AddTransaction(Transaction transaction)
        {
            if (transaction.Amount < 0.01)
            {
                throw new CoreException("Transaction amount must be more or equal than 0.01", ErrorCode.Custom);
            }
            Driver driver = mDriverRepository.Single(d => d.Id == transaction.DriverId);
            if (driver == null)
            {
                throw new CoreException("Driver is not exists", ErrorCode.Custom);
            }

            transaction.Driver = driver;
            mTransactionRepository.Add(transaction);
            mTransactionRepository.Commit();

            UpdateDriverBalance(driver, transaction);
            mDriverRepository.Commit();

            return transaction;
        }

        private void UpdateDriverBalance(Driver driver, Transaction transaction)
        {
            if (transaction.Type == TransactionType.Withdrawal)
            {
                driver.Balance -= transaction.Amount;
            }
            else
            {
                driver.Balance += transaction.Amount;
            }
        }

        public IList<Transaction> GetTransactions(long? driverId, TransactionType? transactionType, PaymentMethod? paymentMethod, TransactionGroup? transactionGroup,
            DateTime fromDate, DateTime toDate)
        {
            var transactions = mTransactionRepository
                .List(t => t.Timestamp >= fromDate && t.Timestamp <= toDate &&
                (!driverId.HasValue || driverId.HasValue && t.Driver.Id == driverId) &&
                (!transactionType.HasValue || transactionType.HasValue && t.Type == transactionType) &&
                (!paymentMethod.HasValue || paymentMethod.HasValue && t.PaymentMethod == paymentMethod) &&
                (!transactionGroup.HasValue || transactionGroup.HasValue && t.Group == transactionGroup));

            return transactions;
        }

        public double GetOrganizationBalance(long organizationId)
        {
            var ordersIds = mOrderRepository.List(o => o.Customer.OrganizationId == organizationId).Select(o => o.Id).ToList();
            double balance = 0;
            for (int i = 0; i < ordersIds.Count; i++)
            {
                var orderId = ordersIds[i];
                var transactions = mTransactionRepository.List().Where(t => t.OrderId == orderId).Select(t => t.Amount).ToList();
                for (int j = 0; j < transactions.Count; j++)
                {
                    balance += transactions[j];
                }
            }
            return balance;
        }

        public Transaction CreateCarRentTransaction(long driverId)
        {
            var driver = mDriverRepository.Single(d => d.Id == driverId);

            var transaction = new Transaction
            {
                Amount = driver.DayTimeFee,
                Comment = "Аренда ТС",
                Group = TransactionGroup.Rent,
                DriverId = driver.Id,
                Driver = driver,
                PaymentMethod = PaymentMethod.NonCash,
                Type = TransactionType.Withdrawal
            };

            mTransactionRepository.Add(transaction);
            mTransactionRepository.Commit();

            return transaction;
        }

        public Transaction CreateSupportTransaction(long driverId)
        {
            var driver = mDriverRepository.Single(d => d.Id == driverId);

            var globalSettings = mGlobalSettingsRepository.List().FirstOrDefault();

            if (globalSettings == null)
            {
                return null;
            }

            if (driver.Balance > -driver.BalanceLimit)
            {
                var transaction = new Transaction
                {
                    Amount = globalSettings.TechnicalSupportFeeSize,
                    Comment = "Техническая поддержка",
                    Group = TransactionGroup.TechSupportFee,
                    DriverId = driver.Id,
                    Driver = driver,
                    PaymentMethod = PaymentMethod.NonCash,
                    Type = TransactionType.Withdrawal
                };

                mTransactionRepository.Add(transaction);
                mTransactionRepository.Commit();

                return transaction;
            }

            return null;
        }

        public void RecalculateDriverBalance()
        {
            var drivers = mDriverRepository.List(d => d.State != DriverState.Fired);

            foreach (var driver in drivers)
            {
                var transactions = mTransactionRepository.List(t => t.DriverId == driver.Id);
                var balance = 0d;
                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    if (transaction.Type == TransactionType.Withdrawal)
                    {
                        balance -= transaction.Amount;
                    }
                    else
                    {
                        balance += transaction.Amount;
                    }
                }

                driver.Balance = balance;
                mDriverRepository.Update(driver);
            }

            mDriverRepository.Commit();
        }

        public void Dispose()
        {
            mDriverRepository.Dispose();
            mGlobalSettingsRepository.Dispose();
            mOrderRepository.Dispose();
            mTransactionRepository.Dispose();
        }
    }
}
