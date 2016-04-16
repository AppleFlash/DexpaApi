using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public interface ITransaction
    {
        string Id { get; }

        DateTime Timestamp { get; }

        TransactionType Type { get; }

        TransactionStatus Status { get; }

        IProvider Provider { get;  }

        decimal OriginalExpense { get; }

        decimal Commission { get; }

        decimal FinalExpense { get; }

        IReadOnlyDictionary<string, string> Extra { get; }


    }
}
