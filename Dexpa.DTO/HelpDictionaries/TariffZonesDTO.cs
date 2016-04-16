namespace Dexpa.DTO.HelpDictionaries
{
    public class TariffZonesDTO
    {
        public long Id { get; set; }

        public TariffZonesTypeDTO TariffZoneType { get; set; }

        public double MinuteCost { get; set; }

        public double KilometerCost { get; set; }

        public int MinVelocity { get; set; }

        public bool IsActive { get; set; }
    }
}