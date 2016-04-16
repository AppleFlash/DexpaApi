using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;
using NLog;

namespace Dexpa.Core.Services
{
    public class DriverService : IDriverService
    {
        private IDriverRepository mDriverRepository;

        private ICarRepository mCarRepository;

        private ICarEventRepository mCarEventRepository;

        private IDriverWorkConditionsRepository mWorkConditionsRepository;

        private ITrackPointService mTrackPointService;

        private IDriverScoresRepository mDriverScoresRepository;

        private ICustomerFeedbackRepository mCustomerFeedbackRepository;

        private Logger mLogger = LogManager.GetCurrentClassLogger();

        public DriverService(IDriverRepository driverRepository,
            ICarRepository carRepository,
            IDriverWorkConditionsRepository workConditionsRepository,
            ITrackPointService trackPointService,
            ICarEventRepository carEventRepository, 
            IDriverScoresRepository driverScoresRepository,
            ICustomerFeedbackRepository customerFeedbackRepository)
        {
            mDriverRepository = driverRepository;
            mCarRepository = carRepository;
            mWorkConditionsRepository = workConditionsRepository;
            mTrackPointService = trackPointService;
            mCarEventRepository = carEventRepository;
            mDriverScoresRepository = driverScoresRepository;
            mCustomerFeedbackRepository = customerFeedbackRepository;
        }

        public Driver GetDriver(long id)
        {
            return mDriverRepository.Single(d => d.Id == id);
        }

        public Driver AddDriver(Driver driver)
        {
            UpdateDriverRelations(driver);
            driver = mDriverRepository.Add(driver); ;
            mDriverRepository.Commit();
            return driver;
        }

        public void DeleteDriver(long driverId)
        {
            var driver = mDriverRepository.Single(d => d.Id == driverId);
            if (driver != null)
            {
                mDriverRepository.Delete(driver);
                mDriverRepository.Commit();
            }
        }

        public IList<Driver> GetDrivers(bool includeFiredDrivers = true)
        {
            return mDriverRepository.List(d => includeFiredDrivers || d.State != DriverState.Fired, withNoLock: false);
        }

        public IList<Driver> GetAbleToOrdersDrivers()
        {
            return GetDrivers(false).Where(d => d.Balance > d.BalanceLimit).ToList();
        }

        public IList<Driver> GetActiveDrivers()
        {
            var currentTume = DateTime.UtcNow;
            var drivers = mDriverRepository.List();
            return drivers.Where(d => (currentTume - d.LastTrackUpdateTime).TotalMinutes <= 30).ToList();
        }

        public Driver UpdateDriver(Driver driver)
        {
            UpdateDriverRelations(driver);
            driver = mDriverRepository.Update(driver);
            if (mDriverRepository.IsItemPropertyChanged(driver, "State"))
            {
                mLogger.Debug("Driver state was changed: {0}", driver.State);
            }
            AddTrackPoint(driver);
            mDriverRepository.Commit();
            return driver;
        }

        public Driver BlockUnblockDriver(long driverId, string comment, string implementedById, bool block)
        {
            var driver = mDriverRepository.Single(d => d.Id == driverId);
            if (driver == null || driver.CarId == null)
                return null;

            var carEvent = new CarEvent()
            {
                CarId = driver.CarId.Value,
                Comment = comment,
                ImplementedById = implementedById
            };

            if (block)
            {
                driver.State = DriverState.Blocked;
                carEvent.Name = "Водитель заблокирован";
            }
            else
            {
                driver.State = DriverState.NotAvailable;
                carEvent.Name = "Водитель разблокирован";
            }

            mCarEventRepository.Add(carEvent);
            var updatedDriver = mDriverRepository.Update(driver);
            mDriverRepository.Commit();
            return updatedDriver;
        }

        private void AddTrackPoint(Driver driver)
        {
            if (mDriverRepository.IsItemPropertyChanged(driver, "Location"))
            {
                var location = driver.Location;
                var trackPoint = new TrackPoint
                {
                    Direction = (int)location.Direction,
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                    Speed = location.Speed,
                    Driver = driver,
                    DriverId = driver.Id,
                    DriverState = driver.State
                };

                mTrackPointService.AddTrackPoint(trackPoint);

                driver.LastTrackUpdateTime = DateTime.UtcNow;
            }
        }

        private void UpdateDriverRelations(Driver driver)
        {
            if (driver.CarId == 0)
            {
                driver.CarId = null;
            }

            if (driver.WorkConditionsId == 0)
            {
                driver.WorkConditionsId = null;
            }

            Car car = null;
            if (driver.CarId.HasValue)
            {
                car = mCarRepository.Single(c => c.Id == driver.CarId);
            }

            DriverWorkConditions conditions = null;
            if (driver.WorkConditionsId.HasValue)
            {
                conditions = mWorkConditionsRepository.Single(c => c.Id == driver.WorkConditionsId);
            }

            driver.Car = car;
            driver.WorkConditions = conditions;
        }

        public Driver GetDriverByPhone(long phone)
        {
            return mDriverRepository.Single(d => d.Phones == phone.ToString());
        }

        public IList<SimpleDriverReport> GetOnlineDrivers()
        {
            var drivers = mDriverRepository.List(d=>d.State!=DriverState.Fired);

            return GetSimpleDriversList(drivers);
        }

        public IList<SimpleDriverReport> GetFiredDrivers()
        {
            var drivers = mDriverRepository.List(d => d.State == DriverState.Fired);

            return GetSimpleDriversList(drivers);
        }

        public IList<LightDriverReport> GetLightDrivers()
        {
            var drivers = mDriverRepository.List();
            var cars = mCarRepository.List();
            var workConditions = mWorkConditionsRepository.List();

            var report = new List<LightDriverReport>();

            foreach (var driver in drivers)
            {
                var reportItem = new LightDriverReport();

                reportItem.Id = driver.Id;
                reportItem.FirstName = driver.FirstName;
                reportItem.LastName = driver.LastName;
                reportItem.MiddleName = driver.MiddleName;
                reportItem.Phones = driver.Phones;
                reportItem.State = driver.State;
                reportItem.Balance = driver.Balance;
                reportItem.Timestamp = TimeConverter.UtcToLocal(driver.Timestamp);

                foreach (var car in cars)
                {
                    if (driver.CarId == null)
                    {
                        reportItem.Callsign = "";
                        reportItem.CarModel = "";
                        reportItem.CarRegNumber = "";
                    }
                    if (driver.CarId == car.Id)
                    {
                        reportItem.Callsign = car.Callsign;
                        reportItem.CarModel = car.Brand + " " + car.Model;
                        reportItem.CarRegNumber = car.RegNumber;
                    }
                }

                foreach (var workCondition in workConditions)
                {
                    if (driver.WorkConditionsId == null)
                    {
                        reportItem.WorkConditionId = null;
                        reportItem.WorkConditionName = "";
                    }
                    if (driver.WorkConditionsId == workCondition.Id)
                    {
                        reportItem.WorkConditionId = driver.WorkConditionsId;
                        reportItem.WorkConditionName = workCondition.Name;
                    }
                }

                report.Add(reportItem);
            }

            /*var report = drivers.Join(cars, 
                d => d.CarId, 
                c => c.Id,
                (d, c) => new DriverTableReport()
                {
                    Id=d.Id,
                    Callsign = c.Callsign,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    MiddleName = d.MiddleName,
                    Phones = d.Phones,
                    State = d.State,
                    CarModel = c.Brand + " " + c.Model,
                    CarRegNumber = c.RegNumber,
                    Timestamp = d.Timestamp
                }).ToList();*/

            return report;
        }

        public IList<SimpleDriverReport> GetSimpleDriversList(IList<Driver>  drivers = null)
        {
            //var drivers = mDriverRepository.List();
            if (drivers == null)
            {
                drivers = mDriverRepository.List();
            }
            var cars = mCarRepository.List();

            var report = new List<SimpleDriverReport>();

            foreach (var driver in drivers)
            {
                var reportItem = new SimpleDriverReport();

                long id = driver.Id;

                string callsign = null;

                foreach (var car in cars)
                {
                    if (driver.CarId == null)
                    {
                        callsign = "";
                    }
                    if (driver.CarId == car.Id)
                    {
                        callsign = car.Callsign;
                    }
                }

                reportItem.Id = id;
                reportItem.Name = String.Format("[{0}] - {1} {2} {3}", new string[] { callsign, driver.LastName, driver.FirstName, driver.MiddleName });
                reportItem.Phones = driver.Phones;

                report.Add(reportItem);
            }

            return report;
        }

        public IList<DriverCarReport> GetDriverCarReport()
        {
            var drivers = mDriverRepository.List();
            var cars = mCarRepository.List();

            var report = new List<DriverCarReport>();

            var nowDate = DateTime.UtcNow;

            foreach (var driver in drivers)
            {
                if((nowDate - driver.LastTrackUpdateTime).TotalMinutes > 30)
                    continue;

                var reportItem = new DriverCarReport();

                reportItem.Id = driver.Id;
                reportItem.FirstName = driver.FirstName;
                reportItem.LastName = driver.LastName;
                reportItem.MiddleName = driver.MiddleName;
                reportItem.Phones = driver.Phones;
                reportItem.Location = driver.Location;
                reportItem.LastTrackUpdateTime = TimeConverter.UtcToLocal(driver.LastTrackUpdateTime);
                reportItem.State = driver.State;
                reportItem.IsOnline = driver.IsOnline;

                if (driver.Content != null)
                {
                    var lastContents =
                        driver.Content.Where(c => c.Type == DexpaContentType.Front).OrderBy(c => c.TimeStamp).LastOrDefault();
                    if (lastContents != null)
                    {
                        reportItem.PhotoUrl = lastContents.WebUrl;
                        reportItem.PhotoUrlSmall = lastContents.WebUrlSmall;
                        reportItem.PhotoUrlThumb = lastContents.WebUrlThumb;
                    }
                }

                foreach (var car in cars)
                {
                    if (driver.CarId == null)
                    {
                        reportItem.CarId = null;
                        reportItem.Callsign = "-";
                        reportItem.CarRegNumber = "";
                        reportItem.CarColor = "";
                        reportItem.CarModel = "";
                    }
                    if (driver.CarId == car.Id)
                    {
                        reportItem.CarId = car.Id;
                        reportItem.Callsign = car.Callsign;
                        reportItem.CarRegNumber = car.RegNumber;
                        reportItem.CarColor = car.Color;
                        reportItem.CarModel = car.Brand + " " + car.Model;
                    }
                }

                report.Add(reportItem);
            }

            return report;
        }

        public IList<DriverCarRobot> GetAllLightDrivers()
        {
            var drivers = mDriverRepository.List();
            var cars = mCarRepository.List();

            var report = new List<DriverCarRobot>();

            foreach (var driver in drivers)
            {
                var reportItem = new DriverCarRobot
                {
                    Id = driver.Id,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    MiddleName = driver.MiddleName,
                    Phones = driver.Phones,
                    Location = driver.Location,
                    LastTrackUpdateTime = TimeConverter.UtcToLocal(driver.LastTrackUpdateTime),
                    State = driver.State,
                    IsOnline = driver.IsOnline,
                    OrderRadius = driver.RobotSettings.OrderRadius
                };

                if (driver.Content != null)
                {
                    foreach (var content in driver.Content)
                    {
                        if (content.Type == DexpaContentType.DriverPhoto)
                        {
                            reportItem.PhotoUrl = content.WebUrl;
                            reportItem.PhotoUrlSmall = content.WebUrlSmall;
                            break;
                        }
                    }
                }

                foreach (var car in cars)
                {
                    if (driver.CarId == null)
                    {
                        reportItem.CarId = null;
                        reportItem.Callsign = "-";
                        reportItem.CarRegNumber = "";
                        reportItem.CarColor = "";
                        reportItem.CarModel = "";
                    }
                    if (driver.CarId == car.Id)
                    {
                        reportItem.CarId = car.Id;
                        reportItem.Callsign = car.Callsign;
                        reportItem.CarRegNumber = car.RegNumber;
                        reportItem.CarColor = car.Color;
                        reportItem.CarModel = car.Brand + " " + car.Model;
                    }
                }

                report.Add(reportItem);
            }

            return report;
        }

        public IList<DriverScores> GetDriversRating()
        {
            return mDriverScoresRepository.List();
        }

        public void UpdateDriverRating(List<DriverScores> driverScores)
        {
            var driverScoresList = GetDriversRating().ToList();
            for (int i = 0; i < driverScores.Count; i++)
            {
                DriverScores driverScoresObject;
                if (driverScoresList.Count > 0)
                {
                    driverScoresObject = driverScoresList.Single(d => d.DriverId == driverScores[i].DriverId);
                }
                else
                {
                    driverScoresObject = null;
                }
                if (driverScoresObject == null)
                {
                    AddDriverRating(driverScores[i]);
                }
                else
                {
                    mDriverScoresRepository.Update(driverScores[i]);
                    mDriverScoresRepository.Commit();
                }
            }
        }

        public DriverScores AddDriverRating(DriverScores driverScores)
        {
            var driverRating = mDriverScoresRepository.Add(driverScores);
            mDriverScoresRepository.Commit();
            return driverRating;
        }

        public IList<CustomerFeedback> GetCustomerFeedbacks(List<long> orderIds)
        {
            return mCustomerFeedbackRepository.List(o => orderIds.Contains(o.OrderId));
        }

        public void UpdateCustomerFeedbacks(List<CustomerFeedback> feedbacks)
        {
            var orderIds = feedbacks.Select(o => o.OrderId).ToList();
            var existFeedbacks = GetCustomerFeedbacks(orderIds).ToList();

            foreach (var feedback in feedbacks)
            {
                var existFeedback = existFeedbacks.Single(f => f.OrderId == feedback.OrderId);
                if (existFeedback == null)
                {
                    AddCustomerFeedback(feedback);
                }
            }
        }

        public CustomerFeedback AddCustomerFeedback(CustomerFeedback feedback)
        {
            var addFeedback = mCustomerFeedbackRepository.Add(feedback);
            mCustomerFeedbackRepository.Commit();
            return addFeedback;
        }

        public IList<DriverScores> GetDriverScores(long driverId)
        {
            return mDriverScoresRepository.List(d => d.DriverId == driverId);
        }

        public IList<CustomerFeedback> GetCustomerFeedbacks(long driverId, DateTime fromDate, DateTime toDate)
        {
            return mCustomerFeedbackRepository.List(d => d.DriverId == driverId && d.Date>= fromDate && d.Date<=toDate);
        }

        public void Dispose()
        {
            mCarRepository.Dispose();
            mDriverRepository.Dispose();
            mTrackPointService.Dispose();
            mWorkConditionsRepository.Dispose();

        }
    }
}
