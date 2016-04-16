using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Model.Orders
{
    public class Airport
    {
        /// <summary>
        /// Номер рейса.
        /// </summary>
        public string Flight { get; set; }

        /// <summary>
        /// Идентификатор терминала аэропорта.
        /// </summary>
        public string Terminal { get; set; }
    }
}
