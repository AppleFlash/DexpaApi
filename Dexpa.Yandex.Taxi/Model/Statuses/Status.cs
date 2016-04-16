using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Statuses
{
    public enum Status
    {
        [YAXEnum("free")]
        Free,
        [YAXEnum("busy")]
        Busy,
        [YAXEnum("verybusy")]
        VeryBusy
    }
}
