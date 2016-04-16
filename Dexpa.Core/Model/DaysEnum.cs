using System;

namespace Dexpa.Core.Model
{
    [Flags]
    public enum DaysEnum
    {
        None = 0,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,
        Sunday = 128
    }
}
