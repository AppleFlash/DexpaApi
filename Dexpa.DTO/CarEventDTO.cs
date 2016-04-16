using System;
using System.ComponentModel.DataAnnotations;

namespace Dexpa.Core.Model
{
    public class CarEventDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public long CarId { get; set; }

        public string ImplementedByLogin { get; set; }
        public string ImplementedByName { get; set; }
    }
}