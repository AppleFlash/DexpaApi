using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Values;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Taximeter
{
    public class MeterRule
    {
        [YAXAttributeForClass]
        public MeterType Type { get; set; }

        public decimal Price { get; set; }

        public DistanceOrTimeValue Per { get; set; }

        public DistanceOrTimeValue Prepaid { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Item")]
        public List<Area> Areas { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public SpeedValue StopSpeed { get; set; }
    }
}