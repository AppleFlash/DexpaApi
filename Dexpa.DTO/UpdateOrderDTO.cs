using System;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class UpdateOrderDTO
    {
        public OrderDTO Order { get; set; }

        public string UpdateCancelReason { get; set; }
    }
}
