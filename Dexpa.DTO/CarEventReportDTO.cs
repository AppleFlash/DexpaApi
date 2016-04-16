using System;
using System.ComponentModel.DataAnnotations;

namespace Dexpa.Core.Model
{
    public class CarEventReportDTO
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public int? Mileage { get; set; }

        public long? CarEventId { get; set; }

        public long? RepairId { get; set; }
    }
}