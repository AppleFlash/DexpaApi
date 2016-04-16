using YAXLib;

namespace Yandex.Taxi.Model.Rates.Taximeter
{
    public enum MeterType
    {
        [YAXEnum("time")] Time,
        [YAXEnum("distance")] Distance,
        [YAXEnum("idle_time")] IdleTime,
        [YAXEnum("driving_distance")] DrivingDistance,
        [YAXEnum("price")] Price,
        [YAXEnum("city_remoteness")] CityRemoteness
    }
}