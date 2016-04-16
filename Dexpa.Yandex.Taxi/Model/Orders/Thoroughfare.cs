using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Thoroughfare
    {
        [YAXSerializeAs("ThoroughfareName")]
        public string Name { get; set; }

        /// <summary>
        /// Номер дома (строения, корпуса и т. п.)
        /// </summary>
        public Premise Premise { get; set; }
    }
}
