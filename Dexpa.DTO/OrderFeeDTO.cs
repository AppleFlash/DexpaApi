using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class OrderFeeDTO
    {
        public long Id { get; set; }

        public virtual OrderTypeDTO OrderType { get; set; }

        public double Value { get; set; }

        public OrderFeeType FeeType { get; set; }
    }
}
