using System;
using System.ComponentModel.DataAnnotations;

namespace Dexpa.Core.Model
{
    public class NewsMessageDTO
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public bool IsSend { get; set; }

        public DateTime TimeStamp { get; set; }

        public string AuthorLogin { get; set; }
    }
}
