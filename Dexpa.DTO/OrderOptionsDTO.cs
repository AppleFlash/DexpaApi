using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class OrderOptionsDTO
    {
        public CarFeaturesDTO CarFeatures { get; set; }

        public ChildrenSeatDTO ChildrenSeat { get; set; }

        public OrderOptionsDTO()
        {
            this.CarFeatures = new CarFeaturesDTO();
            this.ChildrenSeat = new ChildrenSeatDTO();
        }
    }
}