using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;

namespace Dexpa.Core.Repositories
{
    public interface IDriverRepository : ICRUDRepository<Driver>
    {
        List<DriverTimeReport> GetDriverReport(long? driverId, DateTime fromDate, DateTime toDate);
    }
}
