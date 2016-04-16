using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class RobotLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public bool RobotEnabled { get; set; }

        public double RobotDistance { get; set; }

        /// <summary>
        /// Robot settings. Time before order.
        /// </summary>
        public double RobotTime { get; set; }

        public double? OrderDistance { get; set; }

        public double OrderTime { get; set; }

        public virtual Order Order { get; set; }

        public long OrderId { get; set; }

        public virtual Driver Driver { get; set; }

        public long DriverId { get; set; }

        public bool IsDriverOptionsFit { get; set; }

        public bool IsDriverWorkAllowed { get; set; }

        public bool IsDriverSelected { get; set; }

        public DriverState DriverState { get; set; }

        public bool IsDriverOnline { get; set; }

        [NotMapped]
        public RobotVerdict Verdict
        {
            get
            {
                if (DriverState != DriverState.ReadyToWork)
                    return RobotVerdict.DriverNotReadyToWork;
                else if (!IsDriverWorkAllowed)
                    return RobotVerdict.WorkNotAllowed;
                else if (!IsDriverOnline)
                    return RobotVerdict.DriverIsOffline;
                else if (!RobotEnabled)
                    return RobotVerdict.RobotDisabled;
                else if (!IsDriverOptionsFit)
                    return RobotVerdict.NotFitOrderOptions;
                else if (OrderDistance > RobotDistance)
                    return RobotVerdict.DistanceNotMatch;
                else if (OrderTime > RobotTime)
                    return RobotVerdict.TimeNotMatch;
                else
                    return RobotVerdict.Fit;
            }
        }

        public RobotLog()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
