using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Reports
{
    public class OrganizationOrdersReport
    {
        public long OrderId { get; set; }

        public string OrganizationName { get; set; }

        public string OrderDate { get; set; }

        public string OrderTime { get; set; }

        public double TaxometrAmount { get; set; }

        public string TariffName { get; set; }

        public OrderStateType OrderState { get; set; }

        public string SlipNumber { get; set; }

        public string Creator { get; set; }

        public string Customer { get; set; }

        public string Passenger { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string Driver { get; set; }

        public string Car { get; set; }

        public string CarNumber { get; set; }

        public string Comment { get; set; }
    }
}
