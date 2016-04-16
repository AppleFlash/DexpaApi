using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class TariffZone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public TariffZoneType TariffZoneType { get; set; }

        public double MinuteCost { get; set; }

        public double KilometerCost { get; set; }

        public int MinVelocity { get; set; }

        public bool IsActive { get; set; }
    }
}
