using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class CarEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public long CarId { get; set; }

        public virtual User ImplementedBy { get; set; }
        public string ImplementedById { get; set; }

        public CarEvent()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}