using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Requirement
    {
        [YAXSerializeAs("name")]
        [YAXAttributeForClass]
        public RequirementType Type { get; set; }

        [YAXValueForClass]
        public string Value { get; set; }
    }
}
