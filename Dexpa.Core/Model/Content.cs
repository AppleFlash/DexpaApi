using System;
using System.ComponentModel.DataAnnotations;

namespace Dexpa.Core.Model
{
    //TODO: Should be moved to separate db.
    public class Content
    {
        [Key]
        public string Id { get; set; }

        public virtual DexpaContentType Type { get; set; }

        public string WebUrl { get; set; }

        public string WebUrlSmall { get; set; }

        public string WebUrlThumb { get; set; }

        public DateTime TimeStamp { get; set; }

        public virtual long? DriverId { get; set; }

        public virtual long? RepairId { get; set; }

    }
}
