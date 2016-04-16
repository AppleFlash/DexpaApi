using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Premise
    {
        /// <summary>
        /// Номер дома в свободной форме.
        /// </summary>
        [YAXSerializeAs("PremiseNumber")]
        public string Number { get; set; }
    }
}
