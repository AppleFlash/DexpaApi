using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.DTO
{
    public class WayBillsDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual DriverDTO Driver { get; set; }

        public virtual CarDTO Car { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int StartMileage { get; set; }

        public int EndMileage { get; set; }

        public string Responsible { get; set; }

        public double Period { get; set; }

        public double MaxPeriod { get; set; }
    }
}
