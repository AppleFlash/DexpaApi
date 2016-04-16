using System;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class TransactionDTO
    {
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Amount { get; set; }

        public TransactionTypeDTO Type { get; set; }

        public PaymentMethodDTO PaymentMethod { get; set; }

        public string Comment { get; set; }

        public TransactionGroupDTO Group { get; set; }

        public DriverDTO Driver { get; set; }

        public virtual OrderDTO Order { get; set; }

        public long? OrderId { get; set; }
    }
}