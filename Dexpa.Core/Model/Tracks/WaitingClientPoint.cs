using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Tracks
{
    public class WaitingClientPoint
    {
        public int FreeWaiting { get; set; }

        public int PaidWaiting { get; set; }

        public double WaitingCost { get; set; }

        public long PointId { get; set; }

        public long OrderId { get; set; }

        public long Delay { get; set; }
    }
}
