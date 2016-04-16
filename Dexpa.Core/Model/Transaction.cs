using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public double Amount { get; set; }

        public TransactionType Type { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        
        public string Comment { get; set; }

        public TransactionGroup Group { get; set; }

        public virtual Driver Driver { get; set; }

        public long DriverId { get; set; }

        public virtual Order Order { get; set; }

        public long? OrderId { get; set; }

        public string QiwiTransactionId { get; set; }

        public DateTime InsertDate { get; private set; }

        public Transaction()
        {
            Timestamp = DateTime.UtcNow;
            InsertDate = Timestamp;
        }

        public Transaction(DateTime dateTime)
        {
            Timestamp = dateTime;
            InsertDate = DateTime.UtcNow;
        }
    }
}
