using System;
using System.Collections.Generic;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class OrderDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string FromAddress
        {
            get
            {
                if (FromAddressDetails == null)
                {
                    return string.Empty;
                }
                return FromAddressDetails.FullName;
            }
        }

        public string ToAddress
        {
            get
            {
                if (ToAddressDetails == null)
                {
                    return string.Empty;
                }
                return ToAddressDetails.FullName;
            }
        }

        public AddressDTO FromAddressDetails { get; set; }

        public AddressDTO ToAddressDetails { get; set; }

        public DateTime DepartureDate { get; set; }

        public double Cost { get; set; }

        public DriverDTO Driver { get; set; }

        public CustomerDTO Customer { get; set; }

        public OrderStateDTO State { get; set; }

        public string StateMessage { get; set; }

        public DateTime? AcceptTime { get; set; }

        public DateTime? StartWaitTime { get; set; }

        public OrderSourceDTO Source { get; set; }

        public string SourceOrderId { get; set; }

        public long? TariffId { get; set; }

        public string TariffShortName { get; set; }

        public string Comments { get; set; }

        public byte Discount { get; set; }

        public OrderOptionsDTO OrderOptions { get; set; }

        public double MinCost { get; set; }

        public bool IsAirport { get; set; }

        public double PreliminaryCost { get; set; }
    }
}
