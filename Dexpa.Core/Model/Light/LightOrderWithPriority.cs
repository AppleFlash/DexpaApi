using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model.Light
{
    public class LightOrderWithPriority
    {
        public LightOrder Order { get; set; }
        public OrderPriority Priority { get; set; }

        public LightOrderWithPriority()
        {
            Order = new LightOrder();
        }
    }
}

