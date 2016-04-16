using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts.Tracks
{
    public interface ITrack
    {
        /// <summary>
        /// Driver Id
        /// </summary>
        string Uuid { get; }

        IEnumerable<Point> Points { get; }
    }
}
