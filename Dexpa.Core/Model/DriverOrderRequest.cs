using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class DriverOrderRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public long DriverId { get; set; }

        public virtual Driver Driver { get; set; }

        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        public OrderRequestState State { get; set; }

        public DriverOrderRequest()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
