using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Taximeter;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Services
{
    public class TaximeterService : BaseService
    {
        public TaximeterService() : base(ServiceType.Taximeter)
        {
            TaximeterCalculations = new List<TaximeterCalc>();
        }

        [YAXAttributeForClass]
        [YAXSerializeAs("CalcRule")]
        public TaximeterCalcRule CalculationRule { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "TaximeterCalc")]
        public List<TaximeterCalc> TaximeterCalculations { get; private set; }
    }
}