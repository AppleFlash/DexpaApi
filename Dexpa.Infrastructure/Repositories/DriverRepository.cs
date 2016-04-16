using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class DriverRepository : ARepository<Driver>, IDriverRepository
    {
        public DriverRepository(DbContext context)
            : base(context)
        {
        }

        public List<DriverTimeReport> GetDriverReport(long? driverId, DateTime fromDate, DateTime toDate)
        {
            var report = ((ModelContext)mContext).GetDriverReport(driverId, fromDate, toDate);

            var driverIds = report.Select(r => r.DriverId).ToList();
            var drivers = List(d => driverIds.Contains(d.Id));

            double secondsPerHour = 60 * 60;
            var results = report
                .Select(r => new DriverTimeReport
                {
                    BusyTime = r.NotAvailableDuration / secondsPerHour,
                    OnOrderTime = r.BusyDuration / secondsPerHour,
                    FreeTime = r.ReadyToWorkDuration / secondsPerHour,
                    Date = r.Date,
                    DriverId = r.DriverId
                })
                .ToList();

            if (!driverId.HasValue)
            {
                results = results
                    .GroupBy(r => r.DriverId)
                    .Select(r => new DriverTimeReport
                    {
                        BusyTime = r.Average(a => a.BusyTime),
                        OnOrderTime = r.Average(a => a.OnOrderTime),
                        FreeTime = r.Average(a => a.FreeTime),
                        DriverId = r.Key
                    })
                    .ToList();
            }

            foreach (var item in results)
            {
                item.Efficiency = (int)(item.OnOrderTime / (item.OnOrderTime + item.FreeTime) * 100);
                item.OnlineTime = item.OnOrderTime + item.FreeTime + item.BusyTime;

                var driver = drivers.FirstOrDefault(d => d.Id == item.DriverId);

                item.DriverName = driver != null
                    ? string.Format("{0} {1} {2}", driver.LastName, driver.FirstName, driver.MiddleName)
                    : null;
            }

            return results;
        }
    }
}
