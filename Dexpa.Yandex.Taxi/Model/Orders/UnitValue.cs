using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class UnitValue
    {
        [YAXAttributeForClass]
        [YAXSerializeAs("unit")]
        public Unit Unit { get; set; }

        [YAXValueForClass]
        public decimal Value { get; set; }
    }
}
