namespace Yandex.Taxi.Model.Rates.Services
{
    public class AnimalTransportService : BaseService
    {
        public AnimalTransportService() : base(ServiceType.AnimalTransport)
        {
        }

        public decimal MinPrice { get; set; }
    }
}