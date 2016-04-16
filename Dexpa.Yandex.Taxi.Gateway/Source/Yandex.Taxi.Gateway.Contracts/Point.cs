using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    public class Point
    {
        public decimal Latitude { get; private set; }

        public decimal Longitude { get; private set; }

        public Point(decimal dLatitude, decimal dLongitude)
        {
            this.Latitude = dLatitude;
            this.Longitude = dLongitude;
        }
    }
}
