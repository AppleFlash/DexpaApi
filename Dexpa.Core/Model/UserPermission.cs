using System;

namespace Dexpa.Core.Model
{
    [Flags]
    public enum UserPermission
    {
        ViewDrivers = 0x001,
        EditDrivers = 0x002,
        ViewOrders = 0x004,
        EditOrders = 0x008,
        ViewTransactions = 0x010,
        ViewCars = 0x020,
        EditCars = 0x040,
        ViewDriverWorkConditions = 0x080,
        EditDriverWorkConditions = 0x100,
        ViewCustomers = 0x200,
        EditCustomers = 0x400,

        ViewEditAll = 0xFFF,
    }
}
