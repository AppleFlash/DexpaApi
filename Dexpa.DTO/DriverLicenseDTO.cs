using System;

namespace Dexpa.DTO
{
    public class DriverLicenseDTO
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
