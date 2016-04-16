using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts.Tracks
{
    public class Point
    {
        public decimal Latitude { get; private set; }

        public decimal Longitude { get; private set; }

        public int AverageSpeed { get; private set; }

        public int Direction { get; private set; }

        public DateTime Time {get; private set;}

        public Point(decimal dLatitude, decimal dLongitude, int iAverageSpeed, int iDirection, DateTime dtTime)
        {
            this.Latitude = dLatitude;
            this.Longitude = dLongitude;
            this.AverageSpeed = iAverageSpeed;
            this.Direction = iDirection;
            this.Time = dtTime;
        }
    }
}
