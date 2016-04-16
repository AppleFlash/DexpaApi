using System.Collections.Generic;

namespace Dexpa.DTO
{
    public class DriverWorkConditionsDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<OrderFeeDTO> OrderFees { get; set; }

        public DriverWorkConditionsDTO()
        {
            OrderFees = new List<OrderFeeDTO>();
        }
    }
}
