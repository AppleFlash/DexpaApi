using YAXLib;

namespace Yandex.Taxi.Model.Rates.Units
{
    public enum SpeedUnit
    {
        [YAXEnum("km/h")] KilometerPerHour,
        [YAXEnum("m/s")] MeterPerSecond
    }
}