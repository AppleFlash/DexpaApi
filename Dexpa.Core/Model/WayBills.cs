using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class WayBills
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual Driver Driver { get; set; }

        public long DriverId { get; set; }

        public virtual Car Car { get; set; }

        public long CarId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int StartMileage { get; set; }

        public int EndMileage { get; set; }

        public string Responsible { get; set; }

        public WayBills()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
