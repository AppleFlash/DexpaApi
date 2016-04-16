using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public class OrderPoint
    {
        public long OrderId { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public DateTime Date { get; set; }
        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public long? PointId { get; set; }
    }
}
