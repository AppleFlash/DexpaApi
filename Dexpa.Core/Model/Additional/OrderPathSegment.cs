using System.Collections.Generic;

namespace Dexpa.Core.Model.Additional
{
    public class OrderPathWithTariff
    {
        public long TariffId { get; set; }

        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public List<OrderPathSegment> Segments { get; set; }
    }

    public class OrderPathSegment
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double SegmentLength { get; set; }

        public double Time { get; set; }
    }
}
