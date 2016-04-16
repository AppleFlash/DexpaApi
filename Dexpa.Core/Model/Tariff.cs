using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dexpa.Core.Model
{
    public class Tariff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

        public DaysEnum Days { get; set; }

        public double MinimumCost { get; set; }

        public int IncludeMinutes { get; set; }

        public int IncludeKilometers { get; set; }

        public bool IncludeMinutesAndKilometers { get; set; }

        public int FreeWaitingMinutes { get; set; }

        public int PaidWaitingCost { get; set; }

        public virtual List<TariffZone> TariffZones { get; internal set; }

        public virtual List<TariffRegionCost> RegionsCosts { get; internal set; }

        public TariffOptions TariffOptions { get; set; }

        public DateTime Timestamp { get; set; }

        public string Comment { get; set; }

        public string Uuid { get; set; }

        public string YandexId { get; set; }

        public int RegionIncludedFreeMinutes { get; set; }

        public int AirportFreeWaiting { get; set; }

        public Tariff()
        {
            Timestamp = DateTime.UtcNow;
            TariffZones = new List<TariffZone>();
            RegionsCosts = new List<TariffRegionCost>();
            //FillZones();
        }

        private void FillZones()
        {
            foreach (TariffZoneType zoneType in Enum.GetValues(typeof (TariffZoneType)))
            {
                var zone = new TariffZone
                {
                    IsActive = false,
                    TariffZoneType = zoneType
                };
                TariffZones.Add(zone);
            }
        }
    }
}
