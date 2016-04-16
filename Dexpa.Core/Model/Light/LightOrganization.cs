using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Light
{
    public class LightOrganization
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public string TariffName { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Codeword { get; set; }
    }
}
