namespace Dexpa.DTO
{
    public class RobotSettingsDTO
    {
        public bool Enabled { get; set; }

        public int OrderRadius { get; set; }

        public bool Airports { get; set; }

        public bool OrdersSequence { get; set; }

        public bool WantToHome { get; set; }

        public string AddressSearch { get; set; }

        public int MinutesDepartureTime { get; set; }
    }
}
