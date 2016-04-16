using System.Collections.Generic;
using Yandex.Taxi.Gateway.Contracts.Tracks;

namespace Dexpa.OrdersGateway.Models
{
    class YTracks : ITracks
    {
        public IEnumerable<ITrack> Tracks { get; private set; }

        public YTracks(IEnumerable<ITrack> tracks)
        {
            Tracks = tracks;
        }
    }
}
