using System;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class DriverBlockDTO
    {
        public long DriverId { get; set; }

        public string DispatcherLogin { get; set; }

        public string Comment { get; set; }

        public bool Block { get; set; }
    }
}