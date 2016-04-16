using System;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class CarDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Callsign { get; set; }

        public CarStatusDTO Status { get; set; }

        public string RegNumber { get; set; }

        public int ProductionYear { get; set; }

        public string CarClass { get; set; }

        public string Color { get; set; }

        public ChildrenSeatDTO ChildrenSeat { get; set; }

        public CarFeaturesDTO Features { get; set; }

        public string Description { get; set; }

        public CarPermission Permission { get; set; }

        public string BrandLogo { get; set; }

        public bool BelongsCompany { get; set; }
    }
}