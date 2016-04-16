namespace Yandex.Taxi.Model.Rates.Services
{
    public class ConditionerService : BaseService
    {
        public ConditionerService() : base(ServiceType.Conditioner)
        {
        }

        public decimal MinPrice { get; set; }
    }
}