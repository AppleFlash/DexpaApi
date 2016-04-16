using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Reports
{
    public class OrdersReport
    {
        public long Id { get; set; }

        public string Date { get; set; }

        public double Profit { get; set; }

        public double Rent { get; set; }

        public double TechSupport { get; set; }

        public double Percent { get; set; }

        public double TaxometrAmount { get; set; }

        public int AllOrders { get; set; }

        public int Yandex { get; set; }

        public DateTime Timestamp { get; set; }

        public OrdersReport()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
