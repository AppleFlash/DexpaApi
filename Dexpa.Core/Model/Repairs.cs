using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public float Cost { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public virtual User ImplementedBy { get; set; }

        public string ImplementedById { get; set; }

        public long CarId { get; set; }

        public virtual Driver GuiltyDriver { get; set; }

        public long? GuiltyDriverId { get; set; }

        public virtual ICollection<Content> DamagesPhotos { get; set; }

        public Repair()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}