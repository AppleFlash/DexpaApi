using Yandex.Taxi.Model.Rates.Units;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Values
{
    public class SpeedValue
    {
        [YAXValueForClass]
        public int Value { get; set; }

        [YAXAttributeForClass]
        public SpeedUnit Unit { get; set; }
    }
}