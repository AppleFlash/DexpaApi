using YAXLib;

namespace Yandex.Taxi.Model.Rates.Units
{
    public enum TimeUnit
    {
        [YAXEnum("second")] Second,
        [YAXEnum("minute")] Minute,
        [YAXEnum("hour")] Hour
    }
}