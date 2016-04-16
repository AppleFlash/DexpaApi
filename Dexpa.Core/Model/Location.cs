using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class Location
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Speed { get; set; }

        public double Direction { get; set; }

        public override bool Equals(object obj)
        {
            var target = obj as Location;
            if (target == null)
            {
                return false;
            }
            return IsDoubleEquals(Longitude, target.Longitude) &&
                   IsDoubleEquals(Latitude, target.Latitude) &&
                   IsDoubleEquals(Speed, target.Speed) &&
                   IsDoubleEquals(Direction, target.Direction);
        }

        private bool IsDoubleEquals(double a, double b)
        {
            return Math.Abs(a - b) <= double.Epsilon;
        }
    }
}
