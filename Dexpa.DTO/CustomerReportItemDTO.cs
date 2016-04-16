using System;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class CustomerReportItemDTO
    {
        public long Id { get; set; }

        public Customer Customer { get; set; }

        public int Orders { get; set; }
    }
}
