using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Taximeter;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Services
{
    public class ContinueTransferService : BaseService
    {
        public ContinueTransferService() : base(ServiceType.ContinueTransfer)
        {
            TransferLocations = new List<Area>();
        }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Item")]
        public List<Area> TransferLocations { get; private set; }

        public TaximeterCalc TaximeterCalc { get; set; }
    }
}