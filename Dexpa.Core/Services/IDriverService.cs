using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Services
{
    public interface IDriverService : IDisposable
    {
        Driver GetDriver(long driverId);

        Driver AddDriver(Driver driver);

        void DeleteDriver(long driverId);

        IList<Driver> GetDrivers(bool includeFiredDrivers = true);

        IList<Driver> GetAbleToOrdersDrivers();

        Driver UpdateDriver(Driver driver);

        Driver BlockUnblockDriver(long driverId, string comment, string implementedById, bool block);

        IList<SimpleDriverReport> GetOnlineDrivers();

        IList<SimpleDriverReport> GetFiredDrivers();

        Driver GetDriverByPhone(long phone);

        IList<Driver> GetActiveDrivers();
        
        IList<LightDriverReport> GetLightDrivers();

        IList<SimpleDriverReport> GetSimpleDriversList(IList<Driver> drivers=null);

        IList<DriverCarReport> GetDriverCarReport();
        IList<DriverCarRobot> GetAllLightDrivers();

        IList<DriverScores> GetDriversRating();

        void UpdateDriverRating(List<DriverScores> driverScores);

        DriverScores AddDriverRating(DriverScores driverScores);

        IList<CustomerFeedback> GetCustomerFeedbacks(List<long> orderIds);

        void UpdateCustomerFeedbacks(List<CustomerFeedback> feedbacks);

        CustomerFeedback AddCustomerFeedback(CustomerFeedback feedback);

        IList<DriverScores> GetDriverScores(long driverId);

        IList<CustomerFeedback> GetCustomerFeedbacks(long driverId, DateTime fromDate, DateTime toDate);
    }
}