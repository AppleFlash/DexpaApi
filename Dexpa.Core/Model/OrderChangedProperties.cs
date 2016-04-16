using System;

namespace Dexpa.Core.Model
{
    [Flags]
    public enum OrderChangedProperties
    {
        None = 0,
        FromAddress = 2,
        ToAddress = 4,
        DepartureDate = 8,
        Cost = 16,
        DriverId = 32,
        Customer = 64,
        State = 128,
        TariffId = 256,
        CarFeatures = 512,
        ChildrenSeat = 1024,
        Discount = 2048
    }
}