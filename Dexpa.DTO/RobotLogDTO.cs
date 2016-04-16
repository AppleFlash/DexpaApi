using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.DTO
{
    public class RobotLogDTO
    {
        public DateTime Timestamp { get; set; }

        public bool RobotEnabled { get; set; }

        public double RobotDistance { get; set; }

        public double RobotTime { get; set; }

        public double? OrderDistance { get; set; }

        public double OrderTime { get; set; }

        public long OrderId { get; set; }

        public long DriverId { get; set; }

        public bool IsDriverOptionsFit { get; set; }

        public bool IsDriverWorkAllowed { get; set; }

        public bool IsDriverSelected { get; set; }

        public RobotVerdict Verdict { get; set; }
    }
}
