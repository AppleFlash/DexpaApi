using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class OrderRepository : ARepository<Order>, IOrderRepository
    {
        protected DbSet<OrderDriver> mOrderDriversSet;

        protected DbSet<Customer> mCustomersSet;

        private DbSet<RobotLog> mRobotLogSet;

        public OrderRepository(DbContext context)
            : base(context)
        {
            mOrderDriversSet = mContext.Set<OrderDriver>();
            mRobotLogSet = mContext.Set<RobotLog>();
            mCustomersSet = mContext.Set<Customer>();
        }

        public void LockDrivers(List<long> driverIds, long orderId)
        {
            foreach (var driverId in driverIds)
            {
                mOrderDriversSet.Add(new OrderDriver
                {
                    DriverId = driverId,
                    OrderId = orderId
                });
            }
        }

        public void UnlockDrivers(long orderId, List<long> driverIds = null)
        {
            List<OrderDriver> orderDrivers;

            if (driverIds == null)
            {
                orderDrivers = mOrderDriversSet
                .ToList();
            }
            else
            {
                orderDrivers = mOrderDriversSet
                    .Where(d => driverIds.Contains(d.DriverId))
                    .ToList();
            }

            foreach (var orderDriver in orderDrivers)
            {
                mContext.Entry(orderDriver).State = EntityState.Deleted;
            }
        }

        public List<long> GetAssignedDrivers()
        {
            return mOrderDriversSet
                .Select(d => d.DriverId)
                .Distinct()
                .ToList();
        }

        public YandexOrdersReport GetYandexOrdersReport(long? driverId = null, DateTime? dateTimeFrom = null,
            DateTime? dateTimeTo = null)
        {
            var report = new YandexOrdersReport();

            var orders = mSet
                .Where(o => o.Source == OrderSource.Yandex &&
                    o.Timestamp >= dateTimeFrom && o.Timestamp < dateTimeTo)
                .Select(o => new { o.DepartureDate, o.Timestamp, o.Id, o.State, o.DriverId })
                .ToList();

            var driverOrders = orders.Where(o => driverId == null || o.DriverId == driverId).ToList();
            report.AllOrders = driverOrders.Count;
            foreach (var order in orders)
            {
                var time = (order.DepartureDate - order.Timestamp).TotalMinutes;
                if (time <= 25)
                {
                    report.TermOrdersLowTime++;
                }
                else if (time > 25 && time <= 60)
                {
                    report.TermOrdersMiddleTime++;
                }
                else if (time > 60)
                {
                    report.TermOrdersHighTime++;
                }
            }

            var termOrdersIds = orders
                .Where(o => (o.DepartureDate - o.Timestamp).TotalMinutes <= 25).Select(o => o.Id)
                .ToList();
            var notTermOrdersIds = orders
                .Where(o => (o.DepartureDate - o.Timestamp).TotalMinutes > 25).Select(o => o.Id)
                .ToList();

            report.TermOrdersAssigned = mRobotLogSet
                .Where(rl => termOrdersIds.Contains(rl.OrderId) && rl.IsDriverSelected)
                .Select(rl => rl.OrderId)
                .Distinct()
                .Count();

            report.DontTermOrdersAssigned = mRobotLogSet
                .Where(rl => notTermOrdersIds.Contains(rl.OrderId) && rl.IsDriverSelected)
                .Select(rl => rl.OrderId)
                .Distinct()
                .Count();

            report.TermOrdersApproved = driverOrders.Count(o => termOrdersIds.Contains(o.Id) &&
                (o.State == OrderStateType.Completed || o.State == OrderStateType.Failed));
            report.DontTermOrdersApproved = driverOrders.Count(o => notTermOrdersIds.Contains(o.Id) &&
                (o.State == OrderStateType.Completed || o.State == OrderStateType.Failed));

            return report;
        }

    }
}
