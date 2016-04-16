using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public interface IQiwiWalletLoginResult
    {
        bool Ok { get; }

        string Message { get; }
    }
}
