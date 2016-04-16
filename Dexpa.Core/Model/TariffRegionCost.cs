using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class TariffRegionCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual Region RegionFrom { get; internal set; }

        public long RegionFromId { get; set; }

        public virtual Region RegionTo { get; internal set; }

        public long RegionToId { get; set; }

        public double Cost { get; set; }
    }
}
