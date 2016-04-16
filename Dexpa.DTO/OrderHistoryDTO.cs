using System;
using System.Collections.Generic;

namespace Dexpa.DTO
{
    public class OrderHistoryDTO
    {
        public string Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public string OldValues { get; set; }

        public string MessageType { get; set; }

        public string Comment { get; set; }
    }
}