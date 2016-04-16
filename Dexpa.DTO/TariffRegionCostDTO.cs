using System;

namespace Dexpa.DTO
{
    public class TariffRegionCostDTO
    {
        public long Id { get; set; }

        public long RegionFromId { get; set; }

        public long RegionToId { get; set; }

        public double Cost { get; set; }
    }
}
