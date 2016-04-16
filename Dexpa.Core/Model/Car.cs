using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Callsign { get; set; }

        public CarStatus Status { get; set; }

        public string RegNumber { get; set; }

        public int ProductionYear { get; set; }

        public CarClass CarClass { get; set; }

        public string Color { get; set; }

        public ChildrenSeat ChildrenSeat { get; set; }

        public CarFeatures Features { get; set; }

        public string Description { get; set; }

        public CarPermission Permission { get; set; }

        [NotMapped]
        public string BrandLogoId { get; set; }

        public DateTime Timestamp { get; private set; }

        public bool BelongsCompany { get; set; }

        public Car()
        {
            Timestamp = DateTime.UtcNow;
            Permission = new CarPermission();
        }

        public Car(DateTime timestamp)
        {
            Timestamp = timestamp;
            Permission = new CarPermission();
        }
    }
}
