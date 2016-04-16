using System;
using System.Configuration;
using System.Globalization;
using Dexpa.Core.Utils;

namespace Dexpa.ServiceCore
{
    public class CarRentService : AService
    {
        public TimeSpan mRentTransactionTime;

        public DateTime mLastRentTime;

        public CarRentService()
        {

            using (var context = new OperationContext())
            {
                var settings = context.GlobalSettingsService.GetSettings();

                mRentTransactionTime = settings.RentTransactionTime;
                mRentTransactionTime = TimeConverter.LocalToUtc(mRentTransactionTime);

                mLastRentTime = settings.LastRentCheckTime;
            }
        }

        protected override void WorkIteration()
        {
            var nextTime = mLastRentTime.Date.Add(mRentTransactionTime).AddDays(1);
            DateTime currentTime = DateTime.UtcNow;
            if (currentTime > nextTime)
            {
                mLastRentTime = currentTime;

                using (var context = new OperationContext())
                {
                    var globalSettings = context.GlobalSettingsService.GetSettings();
                    globalSettings.LastRentCheckTime = mLastRentTime;
                    context.GlobalSettingsService.SaveSettings(globalSettings);

                    var drivers = context.DriverService.GetDrivers(false);
                    foreach (var driver in drivers)
                    {
                        if (driver.DayTimeFee > 0 && Utils.IsSameDay(driver.WorkSchedule, currentTime.DayOfWeek))
                        {
                            var transaction = context.TransactionService.CreateCarRentTransaction(driver.Id);
                            if (transaction != null)
                            {
                                mLogger.Debug("Got rent from driver {0}: {1} rub.", driver.Id, driver.DayTimeFee);
                            }

                            if (driver.TechnicalSupport)
                            {
                                var supportTransaction = context.TransactionService.CreateSupportTransaction(driver.Id);
                                if (supportTransaction != null)
                                {
                                    mLogger.Debug("Got TechnicalSupport fee from driver {0}: {1} rub.", driver.Id, globalSettings.TechnicalSupportFeeSize);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
