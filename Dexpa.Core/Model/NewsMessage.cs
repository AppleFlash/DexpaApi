using System;
using System.ComponentModel.DataAnnotations;

namespace Dexpa.Core.Model
{
    public class NewsMessage
    {
        [Key]
        public long Id { get; set; }

        [StringLength(512)]
        public string Message { get; set; }

        public bool IsSend { get; set; }

        public DateTime TimeStamp { get; set; }

        public string AuthorLogin { get; set; }
    }
}
