using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.DTO
{
    public class TariffOptionsDTO
    {
        public double Wifi { get; set; }

        public double ChildrenSeat { get; set; }

        public double Conditioner { get; set; }

        public double StationWagon { get; set; }

        public double WithAnimals { get; set; }

        public double Skis { get; set; }

        public double Smoke { get; set; }

        public double Baggage { get; set; }

        public TariffOptionsDTO()
        {
            Wifi = 100;
            StationWagon = 200;
            Conditioner = 0;
            WithAnimals = 150;
            Skis = 150;
            Baggage = 100;
            ChildrenSeat = 150;
            Smoke = 0;
        }
    }
}
