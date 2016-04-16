namespace Yandex.Taxi.Model.Rates.Services
{
    public class OtherService : BaseService
    {
        public OtherService() : base(ServiceType.Other)
        {
        }

        public LocalizedString Name { get; set; }

        public decimal MinPrice { get; set; }
    }
}