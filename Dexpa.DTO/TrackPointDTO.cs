using System;
using Dexpa.Core.Model;

namespace Dexpa.DTO
{
    public class TrackPointDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Speed { get; set; }

        public int Direction { get; set; }

        public long DriverId { get; set; }

        public DriverState DriverState { get; set; }
    }
}