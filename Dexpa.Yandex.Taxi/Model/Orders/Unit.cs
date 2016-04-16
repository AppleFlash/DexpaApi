using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public enum Unit
    {
        [YAXEnum("second")]
        Second,
        [YAXEnum("meter")]
        Meter
    }
}
