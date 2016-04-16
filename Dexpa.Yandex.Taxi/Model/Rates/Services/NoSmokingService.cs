namespace Yandex.Taxi.Model.Rates.Services
{
    public class NoSmokingService : BaseService
    {
        public NoSmokingService() : base(ServiceType.NoSmoking)
        {
        }

        public decimal MinPrice { get; set; }
    }
}