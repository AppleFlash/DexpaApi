using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Locality
    {
        /// <summary>
        /// Название населенного пункта на русском языке.
        /// </summary>
        [YAXSerializeAs("LocalityName")]
        public string Name { get; set; }

        /// <summary>
        /// Улица (бульвар, проезд и т. п.).
        /// </summary>
        public Thoroughfare Thoroughfare { get; set; }
    }
}
