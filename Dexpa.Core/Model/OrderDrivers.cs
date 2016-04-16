using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class OrderDriver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public Order Order { get; set; }

        public long OrderId { get; set; }

        public Driver Driver { get; set; }

        public long DriverId { get; set; }

        public OrderDriver()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
