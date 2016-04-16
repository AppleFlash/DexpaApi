using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IWayBillsService
    {
        IList<WayBills> GetWayBillses(long? driverId = null, long? carId = null, DateTime? fromDate = null, DateTime? toDate = null, bool isActive = false);

        WayBills GetWayBills(long id);

        WayBills GetDriverWayBills(long driverId);

        WayBills AddWayBills(WayBills wayBills);

        WayBills UpdateWayBills(WayBills wayBills);

        void DeleteWayBills(long id);
    }
}
