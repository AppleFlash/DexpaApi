using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Dexpa.DTO;

namespace Dexpa.Core.Model
{
    public class RepairDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public float Cost { get; set; }

        [StringLength(256)]
        public string Comment { get; set; }

        public string ImplementedByLogin { get; set; }
        public string ImplementedByName { get; set; }

        public long CarId { get; set; }

        public long? GuiltyDriverId { get; set; }
        public DriverDTO GuiltyDriver { get; set; }

        public ICollection<ContentDTO> DamagesPhotos { get; set; }
    }
}