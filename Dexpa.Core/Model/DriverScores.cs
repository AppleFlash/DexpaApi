using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class DriverScores
    {

        [Key]
        public long Id { get; set; }

        public virtual Driver Driver { get; set; }

        public long DriverId { get; set; }

        public double Total { get; set; }

        public double? AverageClientScore { get; set; }

        public double? ClientFeedbacksCount { get; set; }

        public int? DriverLateScore { get; set; }

        public int? CancelledOrders { get; set; }

        public int? FakeWaitings { get; set; }

        public int? TrackQuality { get; set; }

        public int? ExamResult { get; set; }

        public string ExamDate { get; set; }
    }
}
