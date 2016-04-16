using YAXLib;

namespace Yandex.Taxi.Model.Rates.Units
{
    public enum DistanceOrTimeUnit
    {
        [YAXEnum("second")] Second,
        [YAXEnum("minute")] Minute,
        [YAXEnum("hour")] Hour,
        [YAXEnum("meter")] Meter,
        [YAXEnum("kilometer")] Kilometer
    }
}