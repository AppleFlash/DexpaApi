using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string PrivatePhone { get; set; }

        public virtual Organization Organization { get; set; }

        public long? OrganizationId { get; set; }

        public Customer()
        {

        }
    }
}
