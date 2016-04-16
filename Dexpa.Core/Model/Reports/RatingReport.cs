using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Reports
{
    public class RatingReport
    {
        public string DriverName { get; set; }

        public double Rating { get; set; }

        public double AverageRating { get; set; }

        public long Delay { get; set; }

        public long CanceledOrders { get; set; }

        public long FalseOrders { get; set; }

        public long TracksQuality { get; set; }

        public string Categories { get; set; }

        public long ExamResult { get; set; }
    }
}
