using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dexpa.Core.Utils;

namespace Dexpa.Infrastructure
{
    public partial class ModelContext
    {

        public List<DriverTimeItem> GetDriverReport(long? driverId, DateTime fromDate, DateTime toDate)
        {
            var driverIdParameter = new SqlParameter("driverId", SqlDbType.BigInt);
            driverIdParameter.Value = DBNull.Value;
            if (driverId.HasValue)
            {
                driverIdParameter.Value = driverId.Value;
            }

            var fromDateParameter = new SqlParameter("fromDate", SqlDbType.DateTime);
            fromDateParameter.Value = fromDate;

            var toDateParameter = new SqlParameter("toDate", SqlDbType.DateTime);
            toDateParameter.Value = toDate;

            var timeOffsetParameter = new SqlParameter("timeOffset", SqlDbType.Int);
            timeOffsetParameter.Value = TimeConverter.UTC_LOCAL_OFFSET;

            return Database.SqlQuery<DriverTimeItem>("exec dbo.sp_GetDriverTimeReport @driverId, @fromDate, @toDate, @timeOffset",
                driverIdParameter, fromDateParameter, toDateParameter, timeOffsetParameter)
                .ToList();
        }

        public class DriverTimeItem
        {
            public long DriverId { get; set; }

            public DateTime Date { get; set; }

            /// <summary>
            /// Duration in seconds
            /// </summary>
            public int ReadyToWorkDuration { get; set; }


            /// <summary>
            /// Duration in seconds
            /// </summary>
            public int NotAvailableDuration { get; set; }
            /// <summary>
            /// Duration in seconds
            /// </summary>
            public int BusyDuration { get; set; }
        }
    }
}
