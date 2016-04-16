using System;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Qiwi.Parser;
using TransactionType = Dexpa.Core.Model.TransactionType;

namespace Dexpa.ServiceCore
{
    public class QiwiTransactionService : AService
    {
        public DateTime mLastQiwiTime;

        public QiwiTransactionService()
        {
            mIterationPauseMs = 30 * 60 * 1000;

            using (var context = new OperationContext())
            {
                var globalSettings = context.GlobalSettingsService.GetSettings();
                mLastQiwiTime = globalSettings.LastQiwiCheckTime;
                mIterationPauseMs = globalSettings.QiwiCheckInterval * 10 * 1000;
            }
        }

        protected override void WorkIteration()
        {
            mLogger.Debug("Checking qiwi transactions...");
            using (var context = new OperationContext())
            {
                var globalSettings = context.GlobalSettingsService.GetSettings();

                var login = globalSettings.QiwiLogin;
                var password = globalSettings.QiwiPassword;

                var qiwiParcer = new QiwiWalletParser(new QiwiHTMLSourceProvider());
                var loginResult = qiwiParcer.Login(login, password);
                if (loginResult.Ok)
                {
                    mLastQiwiTime = globalSettings.LastQiwiCheckTime;

                    DateTime currentTime = DateTime.UtcNow;
                    var transactionsList = qiwiParcer.GetTransactions(mLastQiwiTime, currentTime,
                        Qiwi.Parser.TransactionType.Income, TransactionStatus.Success);

                    globalSettings.LastQiwiCheckTime = DateTime.UtcNow;
                    context.GlobalSettingsService.SaveSettings(globalSettings);
                    if (!transactionsList.Any())
                    {
                        return;
                    }

                    var drivers = context.DriverService.GetDrivers(true);

                    var minDate = TimeConverter.LocalToUtc(transactionsList.Min(t => t.Timestamp));
                    var maxDate = TimeConverter.LocalToUtc(transactionsList.Max(t => t.Timestamp));

                    var dbTransactions = context.TransactionService.GetTransactions(null, null, PaymentMethod.Qiwi, null,
                        minDate.Date.AddMinutes(-10), maxDate.AddMinutes(10));

                    foreach (var transaction in transactionsList)
                    {
                      if (dbTransactions.Any(t => t.QiwiTransactionId == transaction.Id))
                            continue;

                        var comment = transaction.Provider.Comment;
                        var driver = drivers.FirstOrDefault(
                            d => d.Phones.Split(',').Any(phone =>
                                comment.Contains(phone.Trim()))
                                );


                        if (driver != null)
                        {
                            var amount = (double)transaction.OriginalExpense * (1 - globalSettings.QiwiFee);
                            var timestamp = TimeConverter.LocalToUtc(transaction.Timestamp);
                            var newTransaction = new Transaction(timestamp)
                            {
                                DriverId = driver.Id,
                                PaymentMethod = PaymentMethod.Qiwi,
                                Type = TransactionType.Replenishment,
                                Group = TransactionGroup.Other,
                                Amount = amount,
                                QiwiTransactionId = transaction.Id,
                                Comment = "Транзакция: " + transaction.Id
                            };

                            context.TransactionService.AddTransaction(newTransaction);
                            mLogger.Debug("Added transaction {0} for driver {1}: {2} rub.", newTransaction.QiwiTransactionId, newTransaction.DriverId, newTransaction.Amount);
                        }
                    }
                }
                else
                {
                    mLogger.Debug("Error on Login to Qiwi. {0}", loginResult.Message);
                }
            }
        }
    }
}
