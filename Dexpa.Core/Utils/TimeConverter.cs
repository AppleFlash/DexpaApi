using System;

namespace Dexpa.Core.Utils
{
    public class TimeConverter
    {
        public const int UTC_LOCAL_OFFSET = 3;

        public static DateTime LocalToUtc(DateTime localTime)
        {
            return localTime.AddHours(-UTC_LOCAL_OFFSET);
        }

        public static DateTime? LocalToUtc(DateTime? localTime)
        {
            return localTime.HasValue
                ? LocalToUtc(localTime.Value)
                : (DateTime?)null;
        }

        public static DateTime UtcToLocal(DateTime utcTime)
        {
            return utcTime.AddHours(UTC_LOCAL_OFFSET);
        }

        public static TimeSpan LocalToUtc(TimeSpan localTime)
        {
            if (localTime.TotalHours < UTC_LOCAL_OFFSET)
            {
                localTime = localTime + new TimeSpan(1, 0, 0, 0);//+1 day
            }
            var utcTime = localTime.Add(new TimeSpan(-UTC_LOCAL_OFFSET, 0, 0));
            return utcTime;
        }

        public static TimeSpan UtcToLocal(TimeSpan utcTime)
        {
            var localTime = utcTime.Add(new TimeSpan(UTC_LOCAL_OFFSET, 0, 0));
            if (localTime.TotalDays >= 1)
            {
                localTime = localTime.Add(new TimeSpan(-1, 0, 0, 0));//-1 day
            }
            return localTime;
        }
    }
}