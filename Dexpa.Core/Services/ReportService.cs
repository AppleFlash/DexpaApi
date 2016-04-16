using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Model.Reports;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;

namespace Dexpa.Core.Services
{
    public class ReportService : IReportService
    {
        private IOrderRepository mOrderRepository;
        private IDriverRepository mDriverRepository;
        private ITransactionRepository mTransactionRepository;
        private IOrganizationRepository mOrganizationRepository;
        private IOrderHistoryRepository mOrderHistoryRepository;
        private IRobotLogRepository mRobotLogRepository;
        private ICustomerRepository mCustomerRepository;

        private IOrderService mOrderService;

        public ReportService(IOrderService orderService, IOrderRepository orderRepository,
            IDriverRepository driverRepository, ITransactionRepository transactionRepository,
            IOrganizationRepository organizationRepository, IOrderHistoryRepository orderHistoryRepository,
            IRobotLogRepository robotLogRepository, ICustomerRepository customerRepository)
        {
            mOrderRepository = orderRepository;
            mDriverRepository = driverRepository;
            mTransactionRepository = transactionRepository;
            mOrganizationRepository = organizationRepository;
            mOrderHistoryRepository = orderHistoryRepository;
            mRobotLogRepository = robotLogRepository;
            mCustomerRepository = customerRepository;

            mOrderService = orderService;
        }

        public List<DriversReport> GetDriversReport(DateTime? dateTimeFrom = null, DateTime? dateTimeTo = null, long? driverId = null, long? workConditionsId = null)
        {
            IList<Driver> drivers;
            if (driverId != null)
            {
                drivers = mDriverRepository.List(d => d.Id == driverId);
            }
            else
            {
                drivers = mDriverRepository.List();
            }
            if (workConditionsId != null)
            {
                drivers = drivers.Where(d => d.WorkConditionsId == workConditionsId).ToList();
            }

            var driverIds = drivers.Select(d => d.Id).ToList();

            var transactions = mTransactionRepository
                .List(t => driverIds.Contains(t.DriverId) && t.Timestamp >= dateTimeFrom && t.Timestamp <= dateTimeTo)
                .GroupBy(t => t.DriverId)
                .ToList();

            var ordersInPeriod = mOrderRepository.List(o => o.DriverId != null && o.Timestamp >= dateTimeFrom && o.Timestamp < dateTimeTo);

            var result = new List<DriversReport>();
            foreach (var driverTransactions in transactions)
            {
                var driver = drivers.FirstOrDefault(d => d.Id == driverTransactions.Key);
                var driverOrders = ordersInPeriod.Where(o => o.DriverId == driverTransactions.Key).ToList();

                var orderFeeTransactions = driverTransactions.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.OrderFee).ToList();
                var orderIds = orderFeeTransactions.Select(o => o.OrderId).ToList();
                var completedOrders = mOrderRepository.List(o => orderIds.Contains(o.Id)).ToList();

                var orderIdsToRemove = orderIds.Where(o => driverOrders.All(oo => oo.Id != o)).ToList();
                completedOrders.RemoveAll(o => orderIdsToRemove.Contains(o.Id));

                var item = new DriversReport
                {
                    DriverWorkConditions = driver.WorkConditions != null ? driver.WorkConditions.Name : "",
                    DriverCallsign = driver.Car != null ? driver.Car.Callsign : "",
                    DriverName = string.Format("{0} {1} {2}", driver.LastName, driver.FirstName, driver.MiddleName),
                    TechSupport = driverTransactions.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.TechSupportFee).Sum(t => t.Amount),
                    Rent = driverTransactions.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.Rent).Sum(t => t.Amount),
                    Percent = orderFeeTransactions.Sum(t => t.Amount),
                    TaxometrAmount = completedOrders.Where(o => o.State == OrderStateType.Completed).Sum(o => o.Cost),
                    Id = driver.Id,
                    AllOrders = driverOrders.Count,
                    DoneOrders = driverOrders.Count(o => o.State == OrderStateType.Completed),
                    ClientCanceled = driverOrders.Count(o => o.State == OrderStateType.Canceled),
                    DriverCanceled = driverOrders.Count(o => o.State == OrderStateType.Failed)
                };
                item.Amount = item.TechSupport + item.Rent + item.Percent;

                result.Add(item);
            }

            return result;
        }

        public List<DispatcherReport> GetDispatcherReport()
        {
            IList<Order> ordersList = mOrderRepository.List();

            List<DispatcherReport> dispatcherReports = new List<DispatcherReport>();

            DispatcherReport dispatcherReport = new DispatcherReport();

            dispatcherReports.Add(dispatcherReport);
            return dispatcherReports;
        }

        public List<OrdersReport> GetOrdersReport(DateTime dateFrom, DateTime dateTo)
        {
            List<OrdersReport> ordersReports = new List<OrdersReport>();

            var reportDatesList = new List<DateTime>();

            var rDate = dateFrom;
            dateTo = dateTo.AddMonths(1);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, DateTime.DaysInMonth(dateTo.Year, dateTo.Month), dateTo.Hour,
                    dateTo.Minute, dateTo.Second);

            while (rDate <= dateTo)
            {
                reportDatesList.Add(rDate);
                rDate = rDate.AddMonths(1);
                rDate = new DateTime(rDate.Year, rDate.Month, DateTime.DaysInMonth(rDate.Year, rDate.Month), rDate.Hour,
                    rDate.Minute, rDate.Second);
            }

            for (int i = 0; i < reportDatesList.Count-1; i++)
            {
                var repDateMonthFrom = reportDatesList[i];
                var repDateMonthTo = reportDatesList[i + 1];

                var localRepDate = TimeConverter.UtcToLocal(repDateMonthFrom);

                OrdersReport orderReport = new OrdersReport();

                var transactionsByMonth =
                    mTransactionRepository.List(t => t.Timestamp >= repDateMonthFrom && t.Timestamp < repDateMonthTo);

                orderReport.Date = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(localRepDate.Month);

                var rentList = transactionsByMonth.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.Rent).ToList();
                for (int j = 0; j < rentList.Count; j++)
                {
                    orderReport.Rent += rentList[j].Amount;
                }

                var techSupportList =
                    transactionsByMonth.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.TechSupportFee).ToList();
                for (int j = 0; j < techSupportList.Count; j++)
                {
                    orderReport.TechSupport += techSupportList[j].Amount;
                }

                var percentList =
                    transactionsByMonth.Where(t => t.Type == TransactionType.Withdrawal && t.Group == TransactionGroup.OrderFee).ToList();

                for (int j = 0; j < percentList.Count; j++)
                {
                    orderReport.Percent += percentList[j].Amount;
                }

                var ordersByMonth =
                    mOrderRepository.List(
                        o => o.DriverId != null && o.Timestamp >= repDateMonthFrom && o.Timestamp < repDateMonthTo);

                orderReport.TaxometrAmount =
                    ordersByMonth.Where(o => o.State == OrderStateType.Completed).Sum(o => o.Cost);

                orderReport.Profit = orderReport.Rent + orderReport.TechSupport + orderReport.Percent;

                orderReport.AllOrders = ordersByMonth.Count();

                orderReport.Yandex = ordersByMonth.Count(o => o.Source == OrderSource.Yandex);

                ordersReports.Add(orderReport);
            }

            return ordersReports;
        }

        public List<LightOrder> GetLightSearchOrdersReport(DateTime dateFrom, DateTime dateTo, long? orderId, string customerPhone,
            string fromAddress)
        {
            var ordersInPeriod = mOrderRepository.List(o => (o.Timestamp >= dateFrom && o.Timestamp <= dateTo));
            var customersIds = ordersInPeriod.Select(o => o.CustomerId).ToList();

            var customers = mCustomerRepository.List(c => customersIds.Contains(c.Id));

            var orderWcustomer = ordersInPeriod.Join(customers, o => o.CustomerId, c => c.Id, (o, c) => new
            {
                order = o,
                customer = c
            });

            var orders = orderWcustomer.Where(o =>
                (!orderId.HasValue || o.order.Id == orderId.Value) &&
                (string.IsNullOrEmpty(customerPhone) || o.customer.Phone.Contains(customerPhone)) &&
                (string.IsNullOrEmpty(fromAddress) || o.order.FromAddress.FullName.ToLower().Contains(fromAddress)))
                .Select(o => new OrderWithPriority(o.order, 0));

            var lightOrders = mOrderService.GetLightOrders(orders).Select(o => o.Order).ToList();
            return lightOrders;
        }

        public List<OrganizationOrdersReport> GetOrganizationOrdersReport()
        {
            List<OrganizationOrdersReport> organizationOrdersReports = new List<OrganizationOrdersReport>();

            var organizations = mOrganizationRepository.List().ToList();

            for (int i = 0; i < organizations.Count; i++)
            {
                var organizationId = organizations[i].Id;
                var orders = mOrderRepository.List(o => o.Customer.OrganizationId == organizationId).ToList();

                for (int j = 0; j < orders.Count; j++)
                {
                    OrganizationOrdersReport reportItem = new OrganizationOrdersReport();
                    reportItem.OrderId = orders[j].Id;
                    reportItem.OrganizationName = organizations[i].Name;
                    reportItem.OrderDate = orders[j].DepartureDate.ToShortDateString();
                    reportItem.OrderTime = orders[j].DepartureDate.ToShortTimeString();
                    reportItem.TaxometrAmount = orders[j].Cost;
                    reportItem.TariffName = organizations[i].Tariff.Name;
                    reportItem.OrderState = orders[j].State;
                    reportItem.SlipNumber = organizations[i].SlipNumber;
                    reportItem.Creator = "";
                    reportItem.Customer = "";
                    reportItem.Passenger = "";
                    reportItem.FromAddress = orders[j].FromAddress.FullName;
                    reportItem.ToAddress = orders[j].ToAddress.FullName;
                    if (orders[j].Driver != null)
                    {
                        reportItem.Driver = orders[j].Driver.LastName + " " + orders[j].Driver.FirstName + " " +
                                            orders[j].Driver.MiddleName;

                        if (orders[j].Driver.Car != null)
                        {
                            reportItem.Car = orders[j].Driver.Car.Brand + " " + orders[j].Driver.Car.Model;
                            reportItem.CarNumber = orders[j].Driver.Car.RegNumber;
                        }
                    }
                    reportItem.Comment = orders[j].Comments;
                    organizationOrdersReports.Add(reportItem);
                }
            }

            return organizationOrdersReports;
        }

        public YandexOrdersReport GetYandexOrdersReport(long? driverId = null, DateTime? dateTimeFrom = null, DateTime? dateTimeTo = null)
        {
            return mOrderRepository.GetYandexOrdersReport(driverId, dateTimeFrom, dateTimeTo);

        }

        public List<Order> GetAllOrdersReport(DateTime dateFrom, DateTime dateTo, long? driverId, OrderStateType? state, OrderSource? source)
        {
            return
                mOrderRepository.List(
                    o =>
                        (o.Timestamp >= dateFrom && o.Timestamp <= dateTo) &&
                        (!driverId.HasValue || o.DriverId == driverId.Value) &&
                        (!state.HasValue || o.State == state.Value) && (!source.HasValue || o.Source == source.Value))
                    .ToList();
        }

        public List<LightOrder> GetLightAllOrdersReport(DateTime dateFrom, DateTime dateTo, long? driverId, OrderStateType? state, OrderSource? source)
        {
            var orders =
                mOrderRepository.List(
                    o =>
                        (o.Timestamp >= dateFrom && o.Timestamp <= dateTo) &&
                        (!driverId.HasValue || o.DriverId == driverId.Value) &&
                        (!state.HasValue || o.State == state.Value) && (!source.HasValue || o.Source == source.Value))
                    .Select(o => new OrderWithPriority(o, 0));

            var lightOrders = mOrderService.GetLightOrders(orders).Select(o => o.Order).ToList();

            return lightOrders;
        }

        public List<DriverTimeReport> GetDriverTimeReport(long? driverId, DateTime dateFrom, DateTime dateTo)
        {
            var utcDateTo = TimeConverter.LocalToUtc(dateTo);
            var utcDateFrom = TimeConverter.LocalToUtc(dateFrom);
            return mDriverRepository.GetDriverReport(driverId, utcDateFrom, utcDateTo);
        }

        public List<RatingReport> GetRatingReport(long? driverId)
        {
            IList<Driver> drivers = null;
            if (driverId != null)
            {
                drivers = mDriverRepository.List(d => d.Id == driverId);
            }
            else
            {
                drivers = mDriverRepository.List();
            }

            List<RatingReport> report = new List<RatingReport>();

            for (int i = 0; i < drivers.Count; i++)
            {
                report.Add(new RatingReport()
                {
                    DriverName = drivers[i].LastName + " " + drivers[i].FirstName + " " + drivers[i].MiddleName,
                    Rating = 3.3,
                    AverageRating = 3.0,
                    CanceledOrders = 5,
                    Delay = 2,
                    FalseOrders = 1,
                    Categories = "Эконом",
                    ExamResult = 5,
                    TracksQuality = 10
                });
            }

            return report;
        }

        public void Dispose()
        {
            mDriverRepository.Dispose();
            mOrderHistoryRepository.Dispose();
            mOrderRepository.Dispose();
            mOrganizationRepository.Dispose();
            mTransactionRepository.Dispose();
        }
    }
}
