using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.OrdersGateway.Models
{
    class YDriver : IDriver
    {
        public string Name { get; private set; }

        public string Phone { get; private set; }

        public int BirthYear { get; private set; }

        public string Permit { get; private set; }
    }
}
