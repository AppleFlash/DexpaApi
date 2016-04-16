using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.OrdersGateway.Models
{
    class YDriverProfile : IDriverProfile
    {
        public string Uuid { get; private set; }

        public IEnumerable<string> Tarrifs { get; private set; }

        public IEnumerable<string> RateIds { get; private set; }

        public ICar Car { get; private set; }

        public IDriver Driver { get; private set; }
    }
}
