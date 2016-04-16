using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Point
    {
        [YAXSerializeAs("Lat")]
        public decimal Latitude { get; set; }

        [YAXSerializeAs("Lon")]
        public decimal Longitude { get; set; }
    }
}
