using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IBlacklist
    {
        IEnumerable<string> Phones { get; }
    }
}
