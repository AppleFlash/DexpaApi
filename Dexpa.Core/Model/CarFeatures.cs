using System;

namespace Dexpa.Core.Model
{
    [Flags]
    public enum CarFeatures
    {
        None = 0,
        Economy = 2,
        Comfort = 4,
        Bussiness = 8,
        Minivan = 16,
        Conditioner = 32,
        Smoke = 64,
        WithAnimals = 128,
        StationWagon = 256,
        Wifi = 512,
        Coupon = 1024,
        Receipt = 2048
    }
}