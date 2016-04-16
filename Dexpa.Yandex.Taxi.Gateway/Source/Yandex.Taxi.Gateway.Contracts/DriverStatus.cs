using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public class DriverStatus
    {
        public string Uuid { get; private set; }

        public Status Status { get; private set; }

        public DriverStatus(string sUuid, Status eStatus)
        {
            this.Uuid = sUuid;
            this.Status = eStatus;
        }
    }

    public enum Status
    {
        Free,
        Busy,
        VeryBusy
    }
}
