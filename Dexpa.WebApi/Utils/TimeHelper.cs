using System;

namespace Dexpa.WebApi.Utils
{
    public class TimeHelper
    {
#if OMSK_LOCAL_TIME
        private const int UTC_OFFSET = 6;
#endif
#if !OMSK_LOCAL_TIME
        private const int UTC_OFFSET = 3;
#endif
        public static DateTime LocalToUtc(DateTime localTime)
        {
            return localTime.AddHours(-UTC_OFFSET);
        }

        public static DateTime UtcToLocal(DateTime utcTime)
        {
            return utcTime.AddHours(UTC_OFFSET);
        }
    }
}