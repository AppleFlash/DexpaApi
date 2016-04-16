using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiwi.Parser
{
    [Serializable]
    public class QiwiParserException : Exception
    {
        public QiwiParserException() { }

        public QiwiParserException(string message) : base(message) { }

        public QiwiParserException(string message, Exception inner) : base(message, inner) { }
        protected QiwiParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
