using System;
using System.Collections.Generic;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class CreateOrderDTO : OrderDTO
    {
        public List<DriverDTO> Drivers { get; set; }
    }
}
