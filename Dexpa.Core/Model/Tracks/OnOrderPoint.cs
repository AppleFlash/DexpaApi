using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public class OnOrderPoint
    {
        public double DistanceCity { get; set; }

        public double DistanceOutCity { get; set; }

        public int TimeCity { get; set; }

        public int TimeOutCity { get; set; }

        public double Cost { get; set; }

        public long OrderId { get; set; }

        public long PointId { get; set; }
    }
}
