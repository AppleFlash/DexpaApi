using Yandex.Taxi.Model.Rates.Values;

namespace Yandex.Taxi.Model.Rates.Services
{
    public class WaitingService : BaseService
    {
        public WaitingService() : base(ServiceType.Waiting)
        {
        }

        public TimeValue FreeTime { get; set; }
    }
}