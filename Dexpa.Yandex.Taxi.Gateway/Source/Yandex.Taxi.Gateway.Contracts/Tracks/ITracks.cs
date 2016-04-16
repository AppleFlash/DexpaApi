using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts.Tracks
{
    public interface ITracks
    {
        IEnumerable<ITrack> Tracks { get; }
    }
}
