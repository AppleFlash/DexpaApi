using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class TrackPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Speed { get; set; }

        public int Direction { get; set; }

        public long DriverId { get; set; }

        public Driver Driver { get; set; }

        public DriverState DriverState { get; set; }

        public TrackPoint()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
