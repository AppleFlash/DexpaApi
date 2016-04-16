using System;

namespace Dexpa.Core.Model.Events
{
    public class EventOrderStateChanged
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Order Order { get; set; }

        public OrderStateType OrderState { get; set; }
    }
}
