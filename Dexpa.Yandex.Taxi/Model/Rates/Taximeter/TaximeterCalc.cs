using System.Collections.Generic;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Taximeter
{
    public class TaximeterCalc
    {
        public TaximeterCalc()
        {
            MeterRules = new List<MeterRule>();
        }

        public Payment MinPrice { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Meter")]
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public List<MeterRule> MeterRules { get; private set; }
    }
}