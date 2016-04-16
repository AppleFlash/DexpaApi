using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class DriverWorkConditions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual List<OrderFee> OrderFees { get; set; }

        public DriverWorkConditions()
        {
            OrderFees = new List<OrderFee>();
        }
    }
}
