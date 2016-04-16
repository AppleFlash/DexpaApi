using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO.Light
{
    public class LightTariffDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

        public DaysDTO Days { get; set; }

        public string YandexId { get; set; }

        public double MinimumCost { get; set; }
    }
}
