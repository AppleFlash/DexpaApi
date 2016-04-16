using System;
using System.Collections.Generic;
using System.Web.Http;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Model.Reports;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.DTO.HelpDictionaries;
using Dexpa.WebApi.Utils;
using Dexpa.Core.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class ReportController : ApiControllerBase
    {
        private IOrderService mOrderService;

        private ICustomerService mCustomerService;

        private IReportService mReportService;

        public ReportController(IOrderService orderService, ICustomerService customerService, IReportService reportService)
        {
            mOrderService = orderService;
            mCustomerService = customerService;
            mReportService = reportService;
        }

        [HttpGet]
        public int OrdersCount()
        {
            return mOrderService.GetTotalOrdersCount();
        }

        [HttpGet]
        public int CusotomersCount()
        {
            return mCustomerService.GetTotalCustomersCount();
        }

        [HttpGet]
        public List<DispatcherReportDTO> GetDispatcherReport()
        {
            var report = mReportService.GetDispatcherReport();
            return ObjectMapper.Instance.Map<List<DispatcherReport>, List<DispatcherReportDTO>>(report);
        }

        [HttpGet]
        public List<DriversReportDTO> GetDriversReport(DateTime? dateTimeFrom = null, DateTime? dateTimeTo = null, long? driverId = null, long? workConditionsId = null)
        {
            var fromDate = dateTimeFrom.HasValue ? dateTimeFrom.Value.Date : (DateTime?)null;
            var toDate = dateTimeTo.HasValue ? dateTimeTo.Value.Date.AddDays(1) : (DateTime?)null;
            var fromUtc = TimeConverter.LocalToUtc(fromDate);
            var toUtc = TimeConverter.LocalToUtc(toDate);

            var report = mReportService.GetDriversReport(fromUtc, toUtc, driverId, workConditionsId);
            return ObjectMapper.Instance.Map<List<DriversReport>, List<DriversReportDTO>>(report);
        }

        [HttpGet]
        public List<OrdersReportDTO> GetOrdersReport(DateTime dateFrom, DateTime dateTo)
        {
            var fromUtc = TimeConverter.LocalToUtc(dateFrom);
            var toUtc = TimeConverter.LocalToUtc(dateTo);

            var report = mReportService.GetOrdersReport(fromUtc, toUtc);
            return ObjectMapper.Instance.Map<List<OrdersReport>, List<OrdersReportDTO>>(report);
        }

        [HttpGet]
        public List<OrganizationOrdersReportDTO> GetOrganizationOrdersReport()
        {
            var report = mReportService.GetOrganizationOrdersReport();
            return ObjectMapper.Instance.Map<List<OrganizationOrdersReport>, List<OrganizationOrdersReportDTO>>(report);
        }

        [HttpGet]
        public YandexOrdersReportDTO GetYandexOrdersReport(long? driverId = null, DateTime? dateTimeFrom = null, DateTime? dateTimeTo = null)
        {
            var fromUtc = TimeConverter.LocalToUtc(dateTimeFrom);
            var toUtc = TimeConverter.LocalToUtc(dateTimeTo);
            return ObjectMapper.Instance.Map<YandexOrdersReport, YandexOrdersReportDTO>(
                    mReportService.GetYandexOrdersReport(driverId, fromUtc, toUtc));
        }

        [HttpGet]
        public List<OrderDTO> GetAllOrdersReport(DateTime dateTimeFrom, DateTime dateTimeTo, string driverId = null, string state = null, string source = null)
        {
            long? driverID = null;
            OrderStateType? stateType = null;
            OrderSource? sourceType = null;

            if (driverId != null)
            {
                long outValue;
                driverID = long.TryParse(driverId, out outValue) ? (long?)outValue : null;
            }

            if (state != null)
            {
                OrderStateType outType;
                stateType = OrderStateType.TryParse(state, out outType) ? (OrderStateType?)outType : null;
            }

            if (source != null)
            {
                OrderSource outSource;
                sourceType = OrderSource.TryParse(source, out outSource) ? (OrderSource?)outSource : null;
            }

            var utcTimeFrom = TimeConverter.LocalToUtc(dateTimeFrom.Date);
            var utcTimeTo = TimeConverter.LocalToUtc(dateTimeTo.Date).AddDays(1);

            var orders = mReportService.GetAllOrdersReport(utcTimeFrom, utcTimeTo, driverID, stateType, sourceType);
            var report = ObjectMapper.Instance.Map<List<Order>, List<OrderDTO>>(orders);
            for (int i = 0; i < orders.Count; i++)
            {
                report[i].AcceptTime = SetLastStateTime(orders[i], OrderStateType.Accepted);
                report[i].StartWaitTime = SetLastStateTime(orders[i], OrderStateType.Waiting);
            }
            return report;

        }

        [Route("api/report/AllOrdersReport/Light")]
        [HttpGet]
        public List<LightOrder> GetLightAllOrdersReport(DateTime dateTimeFrom, DateTime dateTimeTo, string driverId = null, string state = null, string source = null)
        {
            long? driverID = null;
            OrderStateType? stateType = null;
            OrderSource? sourceType = null;

            if (driverId != null)
            {
                long outValue;
                driverID = long.TryParse(driverId, out outValue) ? (long?)outValue : null;
            }

            if (state != null)
            {
                OrderStateType outType;
                stateType = OrderStateType.TryParse(state, out outType) ? (OrderStateType?)outType : null;
            }

            if (source != null)
            {
                OrderSource outSource;
                sourceType = OrderSource.TryParse(source, out outSource) ? (OrderSource?)outSource : null;
            }

            var utcTimeFrom = TimeConverter.LocalToUtc(dateTimeFrom.Date);
            var utcTimeTo = TimeConverter.LocalToUtc(dateTimeTo.Date).AddDays(1);

            var orders = mReportService.GetLightAllOrdersReport(utcTimeFrom, utcTimeTo, driverID, stateType, sourceType);
            //for (int i = 0; i < orders.Count; i++)
            //{
            //    report[i].AcceptTime = SetLastStateTime(orders[i], OrderStateType.Accepted);
            //    report[i].StartWaitTime = SetLastStateTime(orders[i], OrderStateType.Waiting);
            //}
            return orders;
        }

        [Route("api/report/SearchOrdersReport/Light")]
        [HttpPost]
        public List<LightOrder> GetLightSearchOrdersReport(OrderSearchDTO searchDto)
        {
            long? orderID = null;
            string customerPhone = null;
            string fromAddress = null;

            if (searchDto.OrderId != null)
            {
                long outValue;
                orderID = long.TryParse(searchDto.OrderId, out outValue) ? (long?)outValue : null;
            }

            if (!string.IsNullOrEmpty(searchDto.CusPhone))
            {
                customerPhone = searchDto.CusPhone;
            }

            if (!string.IsNullOrEmpty(searchDto.FromAddr))
            {
                fromAddress = searchDto.FromAddr.ToLower();
            }

            var utcTimeFrom = TimeConverter.LocalToUtc(searchDto.DateTimeFrom.Date);
            var utcTimeTo = TimeConverter.LocalToUtc(searchDto.DateTimeTo.Date).AddDays(1);

            var orders = mReportService.GetLightSearchOrdersReport(utcTimeFrom, utcTimeTo, orderID, customerPhone, fromAddress);
            //for (int i = 0; i < orders.Count; i++)
            //{
            //    report[i].AcceptTime = SetLastStateTime(orders[i], OrderStateType.Accepted);
            //    report[i].StartWaitTime = SetLastStateTime(orders[i], OrderStateType.Waiting);
            //}
            return orders;
        }

        [HttpGet]
        public List<DriverTimeReport> GetDriverTimeReport(DateTime dateFrom, DateTime dateTo, long? driverId = null)
        {
            var report = mReportService.GetDriverTimeReport(driverId, dateFrom, dateTo);
            return report;
        }

        [HttpGet]
        public List<RatingReport> GetRatingReport(long? driverId = null)
        {
            var report = mReportService.GetRatingReport(driverId);
            return report;
        }

        private OrderHistory GetLastStateTime(Order order, OrderStateType orderState)
        {
            return mOrderService.GetLastStateTime(order.Id, orderState);
        }

        private DateTime? SetLastStateTime(Order order, OrderStateType orderState)
        {
            OrderHistory orderHistory = GetLastStateTime(order, orderState);
            if (orderHistory != null)
            {
                return TimeConverter.UtcToLocal(orderHistory.Timestamp);
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mCustomerService.Dispose();
                mOrderService.Dispose();
                mReportService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
