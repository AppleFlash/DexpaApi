using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;

namespace Dexpa.Core.Repositories
{
    public interface IOrderRepository:ICRUDRepository<Order>
    {
        void LockDrivers(List<long> driverIds, long orderId);

        void UnlockDrivers(long orderId, List<long> driverIds = null);

        List<long> GetAssignedDrivers();

        YandexOrdersReport GetYandexOrdersReport(long? driverId = null, DateTime? dateTimeFrom = null,
            DateTime? dateTimeTo = null);
    }
}
