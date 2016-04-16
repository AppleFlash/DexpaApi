using System;

namespace Dexpa.Core.Model
{
    [Flags]
    public enum EventType
    {
        OrderStateChanged,
        DriverReplaced
    }
}