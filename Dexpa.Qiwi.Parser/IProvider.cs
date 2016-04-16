using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    public interface IProvider
    {
        string Title { get; }

        string OperationNumber { get; }

        string Comment { get; }
    }
}
