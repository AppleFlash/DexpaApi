using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class BookingTime
    {
        /// <summary>
        /// 
        /// </summary>
        [YAXSerializeAs("type")]
        [YAXAttributeForClass]
        public BookingTimeType Type { get; set; }

        [YAXFormat("yyyy-MM-ddTHH:mm")]
        [YAXValueForClass]
        public DateTime Time { get; set; }
    }
}
