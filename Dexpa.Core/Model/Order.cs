using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Dexpa.Core.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; protected set; }

        //public string FromAddress { get; set; }

        //public string ToAddress { get; set; }

        public Address FromAddress { get; set; }

        public Address ToAddress { get; set; }

        public DateTime DepartureDate { get; set; }

        public double Cost { get; set; }

        public virtual Driver Driver { get; internal set; }

        public long? DriverId { get; set; }

        public virtual Customer Customer { get; internal set; }

        public long? CustomerId { get; set; }

        public virtual OrderStateType State { get; set; }

        public DateTime Timestamp { get; private set; }

        public OrderSource Source { get; set; }

        public string SourceOrderId { get; set; }

        public virtual List<OrderHistory> OrderHistories { get; internal set; }

        public virtual Tariff Tariff { get; internal set; }

        public long? TariffId { get; set; }

        public string Comments { get; set; }

        public OrderOptions OrderOptions { get; set; }

        public byte Discount { get; set; }

        public double MinCost { get; set; }

        public double PreliminaryCost { get; set; }

        [NotMapped]
        public bool IsAirport
        {
            get
            {
                return FromAddress != null && FromAddress.IsAirport ||
                       ToAddress != null && ToAddress.IsAirport;
            }
        }

        internal Order(Customer customer)
        {
            Customer = customer;
            State = OrderStateType.Created;
        }

        public Order()
        {
            Timestamp = DateTime.UtcNow;
            OrderOptions = new OrderOptions();
        }
    }
}
