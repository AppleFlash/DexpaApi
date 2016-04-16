using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Country
    {
        /// <summary>
        /// Название страны на русском языке.
        /// </summary>
        [YAXSerializeAs("CountryName")]
        public string Name { get; set; }

        /// <summary>
        /// Населенный пункт.
        /// </summary>
        public Locality Locality { get; set; }
    }
}
