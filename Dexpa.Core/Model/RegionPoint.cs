using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class RegionPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public Region Region { get; set; }

        public long RegionId { get; set; }

        public RegionPoint(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }
        public RegionPoint()
        {
        }
    }
}
