namespace Yandex.Taxi.Model.Rates.Services
{
    public class ChildChairService : BaseService
    {
        public ChildChairService() : base(ServiceType.ChildChair)
        {
        }

        public decimal MinPrice { get; set; }
    }
}