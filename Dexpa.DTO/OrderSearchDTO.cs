using System;
using System.Collections.Generic;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class OrderSearchDTO
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public string OrderId { get; set; }
        public string CusPhone { get; set; }
        public string FromAddr { get; set; }
    }
}