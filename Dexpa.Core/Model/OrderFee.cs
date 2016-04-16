using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class OrderFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual OrderType OrderType { get; set; }

        public double Value { get; set; }

        public OrderFeeType FeeType { get; set; }

        public override int GetHashCode()
        {
            return (int)OrderType;
        }
    }
}
