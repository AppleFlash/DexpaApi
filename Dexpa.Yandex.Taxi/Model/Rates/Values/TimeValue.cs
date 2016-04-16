using Yandex.Taxi.Model.Rates.Units;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Values
{
    public class TimeValue
    {
        [YAXValueForClass]
        public int Value { get; set; }

        [YAXAttributeForClass]
        public TimeUnit Unit { get; set; }
    }
}