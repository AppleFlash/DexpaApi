using System.Collections.Generic;
using Yandex.Taxi.Gateway.Contracts.Tracks;

namespace Dexpa.OrdersGateway.Models
{
    class YTrack : ITrack
    {
        public string Uuid { get; set; }

        public IEnumerable<Point> Points { get; set; }
    }
}
