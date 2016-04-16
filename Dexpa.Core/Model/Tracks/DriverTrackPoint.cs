using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public class DriverTrackPoint
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Direction { get; set; }

        public int Speed { get; set; }

        public TrackPointType PointType { get; set; }

        public DriverState DriverState { get; set; }


    }
}
