namespace Dexpa.DTO
{
    public class SearchResultDTO
    {
        public DriverDTO Driver { get; set; }

        public OrderDTO Order { get; set; }

        public CarDTO Car { get; set; }

        public object MapObject { get; set; }
    }
}