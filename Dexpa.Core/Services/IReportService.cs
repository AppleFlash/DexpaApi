using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Model.Reports;

namespace Dexpa.Core.Services
{
    public interface IReportService : IDisposable
    {
        List<DriversReport> GetDriversReport(DateTime? dateTimeFrom = null, DateTime? dateTimeTo = null,
            long? driverId = null, long? workConditionsId = null);
        List<DispatcherReport> GetDispatcherReport();
        List<OrdersReport> GetOrdersReport(DateTime dateFrom, DateTime dateTo);

        List<Order> GetAllOrdersReport(DateTime dateFrom, DateTime dateTo, long? driverId, OrderStateType? state,
            OrderSource? source);

        List<LightOrder> GetLightAllOrdersReport(DateTime dateFrom, DateTime dateTo, long? driverId,
            OrderStateType? state, OrderSource? source);

        List<LightOrder> GetLightSearchOrdersReport(DateTime dateFrom, DateTime dateTo, long? orderId,
            string customerPhone, string fromAddress);
        List<OrganizationOrdersReport> GetOrganizationOrdersReport();
        YandexOrdersReport GetYandexOrdersReport(long? driverId = null, DateTime? dateTimeFrom = null,
            DateTime? dateTimeTo = null);

        List<DriverTimeReport> GetDriverTimeReport(long? driverId, DateTime dateFrom, DateTime dateTo);

        List<RatingReport> GetRatingReport(long? driverId);
    }
}
