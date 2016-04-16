using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Recipient
    {
        [YAXSerializeAs("loyal")]
        [YAXCustomSerializer(typeof(YesNoSerializer))]
        [YAXAttributeForClass]
        public bool IsLoyal { get; set; }

        [YAXSerializeAs("blacklisted")]
        [YAXCustomSerializer(typeof(YesNoSerializer))]
        [YAXAttributeForClass]
        public bool IsInBlacklist { get; set; }
    }
}
