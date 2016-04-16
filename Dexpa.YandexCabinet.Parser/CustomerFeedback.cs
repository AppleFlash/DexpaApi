using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.YandexCabinet.Parser
{
    public class CustomerFeedback
    {
        public string SourceOrderId { get; set; }

        public short Score { get; set; }

        public string Comment { get; set; }
    }
}
