using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiwi.Parser
{
    public interface IHTMLSourceProvider
    {
        bool Login(string sLogin, string sPassword, out string sMessage);

        string GetTransactions(DateTime dtFrom, DateTime dtTo, string sType = null, string sStatus = null);

    }
}
