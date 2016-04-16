using Yandex.Taxi.Model.Rates.Units;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Values
{
    public class DistanceOrTimeValue
    {
        [YAXValueForClass]
        public decimal Value { get; set; }

        [YAXAttributeForClass]
        public DistanceOrTimeUnit Unit { get; set; }
    }
}