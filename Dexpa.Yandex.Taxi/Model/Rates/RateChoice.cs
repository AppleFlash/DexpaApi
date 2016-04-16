using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public enum RateChoice
    {
        [YAXEnum("start")] Start,
        [YAXEnum("end")] End,
        [YAXEnum("transition")] Transition,
    }
}