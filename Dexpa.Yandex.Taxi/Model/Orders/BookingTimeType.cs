using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public enum BookingTimeType
    {
        /// <summary>
        /// Cрочный заказ (до подачи машины меньше 25 минут).
        /// </summary>
        [YAXEnum("notlater")]
        NotLater,
        /// <summary>
        /// несрочный заказ (до подачи машины больше 25 минут).
        /// </summary>
        [YAXEnum("exact")]
        Exact
    }
}
