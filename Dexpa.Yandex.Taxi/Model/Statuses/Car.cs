using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Statuses
{
    public class Car
    {
        [YAXSerializeAs("uuid")]
        [YAXAttributeForClass]
        public string Uuid { get; set; }

        [YAXSerializeAs("status")]
        [YAXAttributeForClass]
        public Status Status { get; set; }
    }
}
