using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.DTO.Light;

namespace Dexpa.WebApi.Controllers
{
    public class BalanceReportController : ApiControllerBase
    {
        private IDriverService mDriverService;
        private ICarService mCarService;
        private IDriverWorkConditionsService mDriverWorkConditionsService;

        public BalanceReportController(IDriverService driverService, ICarService carService,
            IDriverWorkConditionsService driverWorkConditionsService)
        {
            mDriverService = driverService;
            mCarService = carService;
            mDriverWorkConditionsService = driverWorkConditionsService;
        }

        public List<BalanceDTO> Get(bool includeFired = true)
        {
            var drivers = mDriverService.GetDrivers(includeFired);
            var cars = mCarService.GetCars(false, null);

            var report = drivers.GroupJoin(cars, d => d.CarId, c => c.Id, (d, c) => new
            {
                driver = d,
                car = c
            });

            var wcList = mDriverWorkConditionsService.GetWorkConditions();

            var balances = report
                .Select(d => new BalanceDTO
                {
                    Callsign = d.driver.CarId != null ? d.car.First().Callsign : string.Empty,
                    CarName = d.driver.CarId != null ? d.car.First().Brand + " " + d.car.First().Model : string.Empty,
                    DriverId = d.driver.Id,
                    DriverState = d.driver.State,
                    Name = string.Format("{0} {1} {2}", d.driver.LastName, d.driver.FirstName, d.driver.MiddleName),
                    Balance = d.driver.Balance,
                    MoneyLimit = d.driver.BalanceLimit,
                    RentCost = d.driver.DayTimeFee,
                    WorkConditions = d.driver.WorkConditionsId,
                    Phone = !string.IsNullOrEmpty(d.driver.Phones) ? d.driver.Phones : string.Empty
                })
                .ToList();

            return balances;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mDriverService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}