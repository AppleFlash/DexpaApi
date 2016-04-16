using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.OrdersGateway
{
    public class OrderDriversEventArgs : EventArgs
    {
        public Order Order { get; private set; }

        public List<Driver> Drivers { get; private set; }

        public OrderDriversEventArgs(Order order, List<Driver> drivers)
        {
            Order = order;
            Drivers = drivers;
        }
    }
}
