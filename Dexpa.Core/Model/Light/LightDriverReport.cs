using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Light
{
    public class LightDriverReport:LightDriver
    {
        public DriverState State { get; set; }

        public string CarModel { get; set; }

        public string CarRegNumber { get; set; }

        public long? WorkConditionId { get; set; }

        public string WorkConditionName { get; set; }

        public double Balance { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
