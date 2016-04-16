using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.OrdersGateway.Models
{
    class YDriversProfiles : IDriversProfile
    {
        public IEnumerable<IDriverProfile> DriversProfile { get; private set; }
    }
}
