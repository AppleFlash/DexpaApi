using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dexpa.Core.Model
{
    public class User : IdentityUser
    {
        public string LastName { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public virtual Driver Driver { get; set; }

        public long? DriverId { get; set; }

        public UserRole Role { get; set; }

        public string DriverPassword { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }

        public virtual ICollection<CarEvent> CarEvents { get; set; }

        [NotMapped]
        public UserPermission Permissions { get; set; }
    }
}
