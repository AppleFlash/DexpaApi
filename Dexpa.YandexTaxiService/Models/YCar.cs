using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.OrdersGateway.Models
{
    class YCar:ICar
    {
        public string Model { get; private set; }

        public string Number { get; private set; }

        public string Color { get; private set; }

        public int Year { get; private set; }

        public IEnumerable<CarRequirement> Requirements { get; private set; }
    }
}
