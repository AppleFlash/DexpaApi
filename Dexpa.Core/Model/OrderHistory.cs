using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class OrderHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        public OrderStateType OrderState { get; set; }

        public string Comment { get; set; }

        public OrderChangedProperties ChangedProperty { get; set; }

        public string OldValues { get; set; }

        public HistoryMessageType MessageType { get; set; }

        public OrderHistory() {  }

        public OrderHistory(Order order)
        {
            Timestamp = DateTime.UtcNow;
            OrderId = order.Id;
            Order = order;
            OrderState = order.State;
        }
    }
}
