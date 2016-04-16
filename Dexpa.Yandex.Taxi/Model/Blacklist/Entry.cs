using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Blacklist
{
    public class Entry
    {
        [YAXAttributeForClass]
        [YAXSerializeAs("phone")]
        public string Phone { get; set; }
    }
}
