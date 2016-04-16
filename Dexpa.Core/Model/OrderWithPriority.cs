using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    public class OrderWithPriority
    {
        public Order Order { get; private set; }
        public OrderPriority Priority { get; private set; }

        public OrderWithPriority(Order ord, OrderPriority pri)
        {
            Order = ord;
            Priority = pri;
        }
    }
}
