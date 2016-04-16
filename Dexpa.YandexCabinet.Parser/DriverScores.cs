using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.YandexCabinet.Parser
{
    public class DriverScores
    {
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
