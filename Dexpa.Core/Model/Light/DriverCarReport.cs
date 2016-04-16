using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Light
{
    public class DriverCarReport:LightDriver
    {
        public DriverState State { get; set; }

        public long? CarId { get; set; }

        public string CarRegNumber { get; set; }

        public string CarColor { get; set; }

        public string CarModel { get; set; }

        public Location Location { get; set; }

        public DateTime LastTrackUpdateTime { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoUrlSmall { get; set; }

        public string PhotoUrlThumb { get; set; }

        public bool IsOnline { get; set; }
    }
}
