using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public class TrackData
    {
        public List<DriverTrackPoint> Points { get; set; }

        public List<OnOrderPoint> OnOrderPoints { get; set; }

        public List<OrderPoint> OrderPoints { get; set; }

        public List<WaitingClientPoint> WaitingClientPoints { get; set; }

        //IdlePoints

        public TrackData()
        {
            Points = new List<DriverTrackPoint>();
            OnOrderPoints = new List<OnOrderPoint>();
            OrderPoints = new List<OrderPoint>();
            WaitingClientPoints = new List<WaitingClientPoint>();
        }
    }
}
