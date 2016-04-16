using System;

namespace Dexpa.Core.Services
{
    public class OrderConsistencyException : Exception
    {
        public OrderConsistencyException(string message)
            : base(message)
        {
        }
    }
}