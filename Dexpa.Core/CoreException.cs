using System;

namespace Dexpa.Core
{
    public class CoreException : Exception
    {
        public ErrorCode Code { get; private set; }

        public CoreException()
            : this(ErrorCode.Custom)
        {

        }

        public CoreException(ErrorCode code)
            : base(code.ToString())
        {
            Code = code;
        }

        public CoreException(string message, Exception innerException, ErrorCode code)
            : base(message, innerException)
        {
            Code = code;
        }

        public CoreException(string message, ErrorCode code)
            : base(message)
        {
            Code = code;
        }
    }
}
