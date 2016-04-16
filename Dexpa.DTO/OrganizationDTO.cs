using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.DTO
{
    public class OrganizationDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual TariffDTO Tariff { get; set; }

        public long? TariffId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Codeword { get; set; }

        public string SlipNumber { get; set; }

        public double Balance { get; set; }
    }
}
