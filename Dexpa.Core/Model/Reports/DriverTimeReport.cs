using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Reports
{
    public class DriverTimeReport
    {
        public long DriverId { get; set; }

        public string DriverName { get; set; }

        public DateTime Date { get; set; }

        public double OnlineTime { get; set; }

        public double OnOrderTime { get; set; }

        public double FreeTime { get; set; }

        public double BusyTime { get; set; }

        public int Efficiency { get; set; }
    }
}
