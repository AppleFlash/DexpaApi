using System;
using System.Collections.Generic;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class TariffDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

        public DaysDTO Days { get; set; }

        public double MinimumCost { get; set; }

        public int IncludeMinutes { get; set; }

        public int IncludeKilometers { get; set; }

        public bool IncludeMinutesAndKilometers { get; set; }

        public int FreeWaitingMinutes { get; set; }

        public int PaidWaitingCost { get; set; }

        public List<TariffZonesDTO> TariffZones { get; set; }

        public List<TariffRegionCostDTO> RegionsCosts { get; set; } 

        public DateTime Timestamp { get; set; }

        public string Comment { get; set; }

        public TariffOptionsDTO TariffOptions { get; set; }

        public int RegionIncludedFreeMinutes { get; set; }

        public int AirportFreeWaiting { get; set; }

        public string YandexId { get; set; }

        public TariffDTO()
        {
            TariffOptions = new TariffOptionsDTO();
        }
    }
}