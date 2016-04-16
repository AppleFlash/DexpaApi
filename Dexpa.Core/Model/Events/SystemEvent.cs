using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class SystemEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public long? RelatedItemId { get; set; }

        public EventType Type { get; set; }

        public OrderStateType OrderState { get; set; }
    }
}
