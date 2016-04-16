using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public interface IQiwiWalletDataProvider
    {
        IQiwiWalletLoginResult Login(string sLogin, string sPassword);

        IEnumerable<ITransaction> GetTransactions(DateTime dtFrom, DateTime dtTo, TransactionType? eType = null, TransactionStatus? eStatus = null);
    }
}
