using System;
using System.Collections.Generic;
using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.OrdersGateway.Models
{
    class YBlacklist : IBlacklist
    {
        public IEnumerable<string> Phones { get; private set; }

        public YBlacklist()
        {
            Phones = new List<string>();
        }
    }
}
