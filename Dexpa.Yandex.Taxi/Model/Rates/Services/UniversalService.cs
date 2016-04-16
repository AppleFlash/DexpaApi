namespace Yandex.Taxi.Model.Rates.Services
{
    public class UniversalService : BaseService
    {
        public UniversalService() : base(ServiceType.Universal)
        {
        }

        public decimal MinPrice { get; set; }
    }
}