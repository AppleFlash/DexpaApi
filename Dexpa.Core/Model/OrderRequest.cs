using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class OrderRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string OrderUid { get; set; }

        public string DataJson { get; set; }

        public OrderRequestType Type { get; set; }

        public OrderRequest()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    public enum OrderRequestType
    {
        New,
        Cancelled
    }
}
