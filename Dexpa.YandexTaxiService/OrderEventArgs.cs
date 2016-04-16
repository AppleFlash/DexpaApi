using System;
using Dexpa.Core.Model;

namespace Dexpa.OrdersGateway
{
    public class OrderEventArgs : EventArgs
    {
        public long OrderId { get; private set; }

        public OrderEventArgs(long orderId)
        {
            OrderId = orderId;
        }
    }
}
