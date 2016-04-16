using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.YandexTaxiService;
using NLog;
using Car = Yandex.Taxi.Model.Orders.Car;
using Order = Dexpa.Core.Model.Order;
using YaOrder = Yandex.Taxi.Model.Orders.Order;

namespace Dexpa.OrdersGateway
{
    internal class OrderProcessor
    {
        private readonly int mPauseBeforeNextOrder;

        private Logger mLogger = LogManager.GetLogger("OrderProcessor");

        private readonly int mOrderRequestAdditionalTime;

        private bool mStop;

        public OrderProcessor(int pauseBeforeNextOrder, int orderRequestAdditionalTime)
        {
            mPauseBeforeNextOrder = pauseBeforeNextOrder;
            mOrderRequestAdditionalTime = orderRequestAdditionalTime;
        }

        public List<long> ProcessOrder(YaOrder yaOrder, OperationContext context, Order order, out IList<RobotLog> robotLogs)
        {
            robotLogs = new List<RobotLog>();
            mLogger.Debug("Process order: {0}", yaOrder.Id);
            try
            {
                var driverIds = SelectDrivers(yaOrder.Cars, order, context, out robotLogs);
                mLogger.Debug("Drivers for {0}: {1}", order.SourceOrderId, driverIds.Count);

                return driverIds;
            }
            catch (ThreadAbortException)
            {
                //Do nothing
            }
            catch (Exception exception)
            {
                mLogger.Error(exception);
            }
            return new List<long>();
        }

        private List<long> SelectDrivers(List<Car> cars, Order order, OperationContext context, out IList<RobotLog> robotLogs)
        {
            using (new OperationStopwatch("Driver selection time"))
            {
                var selectDrivers = new List<long>();

                var driverService = context.DriverService;
                var allDrivers = driverService.GetDrivers(false);
                var unlockedDrivers = context.OrderService.GetUnassignedDrivers();
                allDrivers = allDrivers
                    .Where(d => unlockedDrivers.Contains(d.Id))
                    .ToList();
                var checkResults = CheckDrivers(allDrivers, order, cars);

                if (cars != null && cars.Count > 0) //Process hot order
                {
                    cars.Sort((c1, c2) => c1.Distance.CompareTo(c2.Distance));
                    mLogger.Debug("Yandex selected {0} drivers for order {1}", cars.Count, order.SourceOrderId);
                    foreach (var car in cars)
                    {
                        mLogger.Debug("Yandex driver {0} for order {1}", car.Uuid, order.SourceOrderId);
                        var driver = allDrivers.FirstOrDefault(d => d.Uuid == car.Uuid);
                        if (driver != null)
                        {
                            var driverResult = checkResults.FirstOrDefault(r => r.DriverId == driver.Id);

                            mLogger.Debug("Order: {0}. Time: {1}, Distance: {2}", order.SourceOrderId, car.Time,
                                car.Distance);

                            if (driverResult != null && driverResult.Verdict == RobotVerdict.Fit)
                            {
                                driverResult.IsDriverSelected = true;
                                mLogger.Debug("Order was assigned: {0}. Time: {1}, Distance: {2}", order.SourceOrderId,
                                    car.Time, car.Distance);
                                selectDrivers.Add(driver.Id);
                            }
                        }
                        else
                        {
                            mLogger.Debug("There are no drivers with uuid: {0}", car.Uuid);
                        }
                    }
                }
                else
                {
                    var fitResults = checkResults
                        .Where(r => r.Verdict == RobotVerdict.Fit)
                        .OrderBy(r => r.OrderDistance)
                        .ToList();

                    foreach (var fitResult in fitResults)
                    {
                        fitResult.IsDriverSelected = true;
                        mLogger.Debug("Order was assigned: {0}. Time: {1}, Distance: {2}", order.SourceOrderId,
                            fitResult.OrderTime, fitResult.OrderDistance);
                        selectDrivers.Add(fitResult.DriverId);
                    }
                }

                robotLogs = checkResults;

                return selectDrivers;
            }
        }

        private List<RobotLog> CheckDrivers(IList<Driver> allDrivers, Order order, List<Car> cars)
        {
            var logs = new List<RobotLog>();

            var orderOptions = order.OrderOptions;
            var currentTime = DateTime.UtcNow;
            for (int i = 0; i < allDrivers.Count; i++)
            {
                var driver = allDrivers[i];
                var orderDistance = Utils.GetDistance(driver.Location.Latitude, driver.Location.Longitude,
                    order.FromAddress.Latitude, order.FromAddress.Longitude);
                var orderTime = (order.DepartureDate - currentTime).TotalMinutes;

                var car = cars != null ? cars.FirstOrDefault(c => c.Uuid == driver.Uuid) : null;
                if (car != null)
                {
                    orderDistance = (double)car.Distance;
                    orderTime = (double)car.Time;
                }

                var log = new RobotLog
                {
                    DriverId = driver.Id,
                    OrderId = order.Id,
                    IsDriverOptionsFit = driver.IsFitOrder(orderOptions),
                    IsDriverWorkAllowed = driver.IsWorkAllowed(),
                    OrderDistance = orderDistance,
                    OrderTime = orderTime,
                    RobotDistance = driver.RobotSettings.OrderRadius,
                    RobotTime = driver.RobotSettings.MinutesDepartureTime,
                    RobotEnabled = driver.RobotSettings.Enabled,
                    DriverState = driver.State,
                    IsDriverOnline = driver.IsOnline
                };

                logs.Add(log);
            }

            return logs;
        }
    }
}
