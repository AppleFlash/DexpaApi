using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class TariffOptions
    {
        public double Wifi { get; set; }

        public double ChildrenSeat { get; set; }

        public double Conditioner { get; set; }

        public double StationWagon { get; set; }

        public double WithAnimals { get; set; }

        public double Skis { get; set; }

        public double Smoke { get; set; }

        public double Baggage { get; set; }
    }
}
