using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.DTO
{
    public class YandexOrdersReportDTO
    {
        public long AllOrders { get; set; }

        public long TermOrdersLowTime { get; set; } //<25 minutes

        public long TermOrdersMiddleTime { get; set; } //25-60 minutes

        public long TermOrdersHighTime { get; set; } //>60 minutes

        public long TermOrdersAssigned { get; set; }

        public long DontTermOrdersAssigned { get; set; }

        public long TermOrdersApproved { get; set; }

        public long DontTermOrdersApproved { get; set; }
    }
}
