using Dexpa.Core.Model;
using Dexpa.DTO.Light;

namespace Dexpa.DTO
{
    public class BalanceDTO
    {
        public string Callsign { get; set; }

        public long DriverId { get; set; }

        public DriverState DriverState { get; set; }

        public string Name { get; set; }

        public string CarName { get; set; }

        public long? WorkConditions { get; set; }

        public string Phone { get; set; }

        public double RentCost { get; set; }

        public double MoneyLimit { get; set; }

        public double Balance { get; set; }
    }
}