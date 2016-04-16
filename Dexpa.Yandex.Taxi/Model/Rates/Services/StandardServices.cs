using System.Collections.Generic;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Services
{
    public class StandardServices
    {
        public StandardServices()
        {
            OtherServices = new List<OtherService>();
        }

        public TaximeterService TaximeterService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public PadiDispatchService PadiDispatchService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public ContinueTransferService ContinueTransferService { get; set; }

        public WaitingService WaitingService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public ConditionerService ConditionerService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public NoSmokingService NoSmokingService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public ChildChairService ChildChairService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public UniversalService UniversalService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public AnimalTransportService AnimalTransportService { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public List<OtherService> OtherServices { get; private set; }
    }
}