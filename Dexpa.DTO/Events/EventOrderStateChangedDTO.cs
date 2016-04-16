using System;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO.Events
{
    public class EventOrderStateChangedDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public OrderDTO Order { get; set; }

        public OrderStateDTO OrderState { get; set; }
    }
}