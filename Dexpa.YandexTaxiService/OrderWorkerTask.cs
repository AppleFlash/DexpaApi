using System.Collections.Generic;

namespace Dexpa.OrdersGateway
{
    class OrderWorkerTask
    {
        public long OrderId { get; set; }

        public List<long> Drivers { get; set; }

        public OrderWorkerTask(long order, List<long> drivers)
        {
            OrderId = order;
            Drivers = drivers;
        }
    }
}
