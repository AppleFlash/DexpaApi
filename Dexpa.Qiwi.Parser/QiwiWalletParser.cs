using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public class QiwiWalletParser : IQiwiWalletDataProvider
    {
        private readonly IHTMLSourceProvider mSourceProvider;

        public QiwiWalletParser(IHTMLSourceProvider sourceProvider)
        {
            this.mSourceProvider = sourceProvider;
        }

        public IQiwiWalletLoginResult Login(string sLogin, string sPassword)
        {
            string sMessage;
            bool bOk = mSourceProvider.Login(sLogin, sPassword, out sMessage);
            return new QiwiWalletLoginResult(bOk, sMessage);
        }

        public IEnumerable<ITransaction> GetTransactions(DateTime dtFrom, DateTime dtTo, TransactionType? eType = null, TransactionStatus? eStatus = null)
        {
            string sHTML = mSourceProvider.GetTransactions(dtFrom, dtTo, ConvertToString(eType), ConvertToString(eStatus));
            HtmlDocument transactionsPage = new HtmlDocument();
            transactionsPage.LoadHtml(sHTML);

            return ParseTransactions(transactionsPage);
        }

        private static string ConvertToString(TransactionStatus? eStatus)
        {
            if (eStatus.HasValue)
            {
                switch (eStatus.Value)
                {
                    case TransactionStatus.Success:
                        return "SUCCESS";
                    case TransactionStatus.Error:
                        return "ERROR";
                    case TransactionStatus.Processed:
                        return "PROCESSED";
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                return null;
            }
        }

        private static string ConvertToString(TransactionType? eType)
        {
            if (eType.HasValue)
            {
                switch (eType.Value)
                {
                    case TransactionType.Income:
                        return "in";
                    case TransactionType.Expenditure:
                        return "out";
                    default:
                        throw new NotSupportedException();
                }
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<ITransaction> ParseTransactions(HtmlDocument transactionsPage)
        {
            var transactionNodes = SelectTransactionNodes(transactionsPage);
            List<ITransaction> result = new List<ITransaction>();

            if (transactionNodes != null)
            {
                foreach (var transactionNode in transactionNodes)
                {
                    ITransaction transaction = ParseTransaction(transactionNode);
                    result.Add(transaction);
                }
            }

            return result;
        }

        private static ITransaction ParseTransaction(HtmlNode transactionNode)
        {
            TransactionStatus eStatus = ParseTransactionStatus(transactionNode);
            TransactionType eType = ParseTransactionType(transactionNode);
            DateTime dtTimestamp = ParseTransactionTimestamp(transactionNode);
            string sId = ParseTransactionId(transactionNode);
            Dictionary<string, string> extra = ParseExtra(transactionNode);
            decimal dOriginalExpense = ParseOriginalExpense(transactionNode);
            decimal dFinalExpense = ParseFinalExpense(transactionNode);
            decimal dCommission = ParseCommission(transactionNode);
            Provider provider = ParseProvider(transactionNode);

            return new Transaction(sId, dtTimestamp, eType, eStatus, provider, dOriginalExpense, dCommission, dFinalExpense, extra);
        }


        private static decimal ParseCommission(HtmlNode transactionNode)
        {
            var commissionNode = transactionNode.SelectSingleNode("./div[contains(@class, 'IncomeWithExpend')]/div[@class = 'commission']");
            if (commissionNode == null)
            {
                return 0M;
            }
            else
            {
                string sCommission = commissionNode.InnerText;
                return ParseMoney(sCommission);
            }
        }

        private static decimal ParseFinalExpense(HtmlNode transactionNode)
        {
            var finalExpenseNode = transactionNode.SelectSingleNode("./div[contains(@class, 'IncomeWithExpend')]/div[@class = 'cash']");
            return ParseMoney(finalExpenseNode.InnerText);
        }

        private static decimal ParseOriginalExpense(HtmlNode transactionNode)
        {
            var originalExpenseNode = transactionNode.SelectSingleNode("./div[@class = 'originalExpense']/span");
            return ParseMoney(originalExpenseNode.InnerText);
        }


        private static decimal ParseMoney(string sMoney)
        {
            string sTrimmedMoney = sMoney.Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\t", string.Empty)
                .Trim();

            if (string.IsNullOrWhiteSpace(sTrimmedMoney))
            {
                return 0M;
            }

            string[] splittedMoney = sTrimmedMoney.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string sMoneyValue = splittedMoney[0];
            string sCurrency = splittedMoney[1];
            return ParseMoneyValue(sMoneyValue);
        }

        private static decimal ParseMoneyValue(string sMoneyValue)
        {
            decimal dValue = decimal.Parse(sMoneyValue, CultureInfo.GetCultureInfo("ru-Ru"));
            return dValue;
        }

        private static Dictionary<string, string> ParseExtra(HtmlNode transactionNode)
        {
            var extraItems = transactionNode.SelectNodes("./div[@class = 'extra']/div[@class = 'item']");

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var extraItem in extraItems)
            {
                var kvPair = ParseItem(extraItem);
                result.Add(kvPair.Key, kvPair.Value);
            }
            return result;
        }

        private static KeyValuePair<string, string> ParseItem(HtmlNode extraItem)
        {
            string sKey = extraItem.SelectSingleNode("./span[@class = 'key']").InnerText;
            string sValue = extraItem.SelectSingleNode("./span[@class = 'value']").InnerText;
            return new KeyValuePair<string, string>(sKey, sValue);
        }

        private static string ParseTransactionId(HtmlNode transactionNode)
        {
            var transactionIdNode = transactionNode.SelectSingleNode("./div[contains(@class,'DateWithTransaction')]/div[@class = 'transaction']");

            var dataActionAttribute = transactionIdNode.Attributes["data-action"];
            if (dataActionAttribute == null)
            {
                return RemoveTrashFromSting(transactionIdNode.InnerText);
            }
            else
            {
                var extra = ParseExtra(transactionNode);

                string sTransactionId;
                extra.TryGetValue("Transaction:", out sTransactionId);
                return sTransactionId;
            }
        }

        private static DateTime ParseTransactionTimestamp(HtmlNode transactionNode)
        {
            var dateWithTransactionNode = transactionNode.SelectSingleNode("./div[contains(@class,'DateWithTransaction')]");
            string sDate = dateWithTransactionNode.SelectSingleNode("./span[@class = 'date']").InnerText;
            string sTime = dateWithTransactionNode.SelectSingleNode("./span[@class = 'time']").InnerText;

            sDate = RemoveTrashFromSting(sDate);
            sTime = RemoveTrashFromSting(sTime);

            string sDateTime = string.Format("{0} {1}", sDate, sTime);

            return DateTime.ParseExact(sDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static string RemoveTrashFromSting(string inputStr)
        {
            inputStr = inputStr.Replace(" ", "");
            inputStr = inputStr.Replace("\n", "");
            inputStr = inputStr.Replace("\r", "");
            inputStr = inputStr.Replace("\t", "");
            return inputStr;
        }

        private static TransactionType ParseTransactionType(HtmlNode transactionNode)
        {
            var incomeAndExpendNode = transactionNode.SelectSingleNode("./div[contains(@class,'IncomeWithExpend')]");
            string sClassAttribute = incomeAndExpendNode.Attributes["class"].Value;
            return FindType(sClassAttribute);
        }

        private static TransactionType FindType(string sClassAttribute)
        {
            string[] splittedClasses = sClassAttribute.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sClass in splittedClasses)
            {
                TransactionType? type = ConvertToType(sClass);
                if (type.HasValue)
                {
                    return type.Value;
                }
            }
            throw new QiwiParserException("Can't find transaction type in \"class\" attribute of the HTML node");
        }

        private static TransactionType? ConvertToType(string sClass)
        {
            if (string.Compare(sClass, "INCOME", true) == 0)
            {
                return TransactionType.Income;
            }
            else if (string.Compare(sClass, "EXPENDITURE", true) == 0)
            {
                return TransactionType.Expenditure;
            }
            {
                return null;
            }
        }

        private static TransactionStatus ParseTransactionStatus(HtmlNode transactionNode)
        {
            string sClassAttribute = transactionNode.Attributes["class"].Value;
            return FindStatus(sClassAttribute);
        }


        private static TransactionStatus FindStatus(string sClassAttribute)
        {
            string[] splittedClasses = sClassAttribute.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sClass in splittedClasses)
            {
                TransactionStatus? status = ConvertToStatus(sClass);
                if (status.HasValue)
                {
                    return status.Value;
                }
            }
            throw new QiwiParserException("Can't find transaction status in \"class\" attribute of the HTML node");
        }

        private static TransactionStatus? ConvertToStatus(string sClass)
        {
            if (string.Compare(sClass, "status_SUCCESS", true) == 0)
            {
                return TransactionStatus.Success;
            }
            else if (string.Compare(sClass, "status_PROCESSED", true) == 0)
            {
                return TransactionStatus.Processed;
            }
            else if (string.Compare(sClass, "status_ERROR", true) == 0)
            {
                return TransactionStatus.Error;
            }
            else
            {
                return null;
            }
        }

        private static Provider ParseProvider(HtmlNode transactionNode)
        {
            var providerNode = transactionNode.SelectSingleNode("./div[contains(@class, 'ProvWithComment')]");
            string sTitle = providerNode.SelectSingleNode("./div[@class = 'provider']/span[1]").InnerText;
            string sOperationNumber = providerNode.SelectSingleNode("./div[@class = 'provider']/span[@class = 'opNumber']").InnerText;
            string sComment = providerNode.SelectSingleNode("./div[@class = 'comment']").InnerText;
            return new Provider(sTitle, sOperationNumber, sComment);
        }

        private static IEnumerable<HtmlNode> SelectTransactionNodes(HtmlDocument transactionsPage)
        {
            return transactionsPage.DocumentNode.SelectNodes("//div[@data-container-name='item']");
        }


        private class Transaction : ITransaction
        {
            public string Id
            {
                get;
                private set;
            }

            public DateTime Timestamp
            {
                get;
                private set;
            }

            public TransactionType Type
            {
                get;
                private set;
            }

            public TransactionStatus Status
            {
                get;
                private set;
            }

            public IProvider Provider
            {
                get;
                private set;
            }

            public decimal OriginalExpense
            {
                get;
                private set;
            }

            public decimal Commission
            {
                get;
                private set;
            }

            public decimal FinalExpense
            {
                get;
                private set;
            }

            public IReadOnlyDictionary<string, string> Extra
            {
                get;
                private set;
            }

            public Transaction(string sId, DateTime dtTimestamp, TransactionType eType, TransactionStatus eStatus, IProvider provider, decimal dOriginalExpense, decimal dCommission, decimal dFinalExpense, IDictionary<string, string> extra)
            {
                this.Id = sId;
                this.Timestamp = dtTimestamp;
                this.Status = eStatus;
                this.Type = eType;
                this.Provider = provider;
                this.OriginalExpense = dOriginalExpense;
                this.Commission = dCommission;
                this.FinalExpense = dFinalExpense;
                this.Extra = new ReadOnlyDictionary<string, string>(extra);
            }

        }

        private class Provider : IProvider
        {
            public string Title
            {
                get;
                private set;
            }

            public string OperationNumber
            {
                get;
                private set;
            }

            public string Comment
            {
                get;
                private set;
            }

            public Provider(string sTitle, string sOperationNumber, string sComment)
            {
                this.Title = sTitle;
                this.OperationNumber = sOperationNumber;
                this.Comment = sComment;
            }
        }

        private class QiwiWalletLoginResult : IQiwiWalletLoginResult
        {
            public bool Ok
            {
                get;
                private set;
            }

            public string Message
            {
                get;
                private set;
            }

            public QiwiWalletLoginResult(bool bOk, string sMessage)
            {
                this.Ok = bOk;
                this.Message = sMessage;
            }
        }
    }
}
