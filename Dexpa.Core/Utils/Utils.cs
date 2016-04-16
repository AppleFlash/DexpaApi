using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.Core.Utils
{
    public static class Utils
    {
        public static bool IsDriverFitOrder(Driver driver, OrderOptions orderOptions)
        {
            var car = driver.Car;
            if (car == null)
            {
                return true;
            }
            var passChildSeat = orderOptions.ChildrenSeat != ChildrenSeat.None &&
                                car.ChildrenSeat != orderOptions.ChildrenSeat ||
                                orderOptions.ChildrenSeat == ChildrenSeat.None;

            var passAllFeatures = true;
            foreach (CarFeatures feature in Enum.GetValues(typeof(CarFeatures)))
            {
                var featurePass = false;
                var orderHasFeature = orderOptions.CarFeatures.HasFlag(feature);
                if (orderHasFeature && car.Features.HasFlag(feature) || !orderHasFeature)
                {
                    featurePass = true;
                }
                if (!featurePass)
                {
                    passAllFeatures = false;
                    break;
                }
            }
            return passChildSeat && passAllFeatures;
        }
        public static bool IsSameDay(DaysEnum days, DayOfWeek dayOfWeek)
        {
            return days.HasFlag(DaysEnum.Friday) && dayOfWeek == DayOfWeek.Friday ||
                   days.HasFlag(DaysEnum.Monday) && dayOfWeek == DayOfWeek.Monday ||
                   days.HasFlag(DaysEnum.Saturday) && dayOfWeek == DayOfWeek.Saturday ||
                   days.HasFlag(DaysEnum.Sunday) && dayOfWeek == DayOfWeek.Sunday ||
                   days.HasFlag(DaysEnum.Thursday) && dayOfWeek == DayOfWeek.Thursday ||
                   days.HasFlag(DaysEnum.Tuesday) && dayOfWeek == DayOfWeek.Tuesday ||
                   days.HasFlag(DaysEnum.Wednesday) && dayOfWeek == DayOfWeek.Wednesday;
        }

        /// <summary>
        /// Distance in kilometers
        /// </summary>
        /// <param name="fromLat"></param>
        /// <param name="fromLong"></param>
        /// <param name="toLat"></param>
        /// <param name="toLong"></param>
        /// <returns></returns>
        public static double? GetDistance(double? fromLat, double? fromLong, double? toLat, double? toLong)
        {
            //source: http://www.movable-type.co.uk/scripts/latlong.html
            if (fromLat.HasValue && fromLong.HasValue && toLat.HasValue && toLong.HasValue)
            {
                var R = 6371;//Earth radius, km
                var fi1 = ToRadians(fromLat.Value);
                var fi2 = ToRadians(toLat.Value);
                var dFi = ToRadians(toLat.Value - fromLat.Value);
                var dLa = ToRadians(toLong.Value - fromLong.Value);

                var a = Math.Pow(Math.Sin(dFi / 2), 2) +
                        Math.Cos(fi1) * Math.Cos(fi2) *
                        Math.Pow(Math.Sin(dLa / 2), 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var d = R * c;

                return d;
            }
            return null;
        }

        private static double ToRadians(double gradValue)
        {
            return Math.PI / 180 * gradValue;
        }
    }
}
