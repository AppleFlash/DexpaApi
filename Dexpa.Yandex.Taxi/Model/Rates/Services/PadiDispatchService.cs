using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Taximeter;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Services
{
    public class PadiDispatchService : BaseService
    {
        //TODO: Custom serialization
        public Area Source { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "TaximeterCalc")]
        public List<TaximeterCalc> TaximeterCalculations { get; private set; }
        public PadiDispatchService() : base(ServiceType.PaidDispatch)
        {
            this.TaximeterCalculations = new List<TaximeterCalc>();
        }
    }
}