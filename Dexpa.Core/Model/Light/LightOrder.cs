using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Light
{
    public class LightOrder
    {
        public long Id { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public DateTime DepartureDate { get; set; }

        public double Cost { get; set; }

        public long DriverId { get; set; }
        public string DriverName { get; set; }

        public string DriverCallsign { get; set; }

        public string DriverPhone { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public bool IsOrganization { get; set; }

        public bool IsYandex { get; set; }

        public bool IsFreeWaitOver { get; set; }
        public OrderStateType State { get; set; }

        public string TariffName { get; set; }

        public DateTime? StartWaitTime { get; set; }

        public DateTime LastHistoryTime { get; set; }
    }
}
