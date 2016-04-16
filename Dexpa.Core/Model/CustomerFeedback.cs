using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class CustomerFeedback
    {
        [Key]
        public long Id { get; set; }

        public virtual Order Order { get; set; }

        public long OrderId { get; set; }

        public virtual Driver Driver { get; set; }

        public long DriverId { get; set; }

        public virtual Customer Customer { get; set; }

        public long? CustomerId { get; set; }

        public DateTime Date { get; set; }

        public short Score { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }
    }
}
