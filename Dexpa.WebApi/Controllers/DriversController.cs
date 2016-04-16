using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Dexpa.WebApi.Controllers
{
    public class DriversController : ApiControllerBase
    {
        private IDriverService mDriverService;


        private IAccountService mAccountService;

        public UserManager<User> UserManager { get; private set; }

        public DriversController(IDriverService driverService, IAccountService accountService)
        {
            mDriverService = driverService;
            mAccountService = accountService;
            UserManager = Startup.UserManagerFactory();
        }

        public IEnumerable<DriverDTO> GetAllDrivers()
        {
            return ObjectMapper.Instance.Map<IList<Driver>, List<DriverDTO>>(mDriverService.GetDrivers());
        }

        public IHttpActionResult GetDriver(long id)
        {
            var driver = mDriverService.GetDriver(id);
            if (driver != null)
            {
                var dDto = ObjectMapper.Instance.Map<Driver, DriverDTO>(driver);
                var d = mUserManager.Users.SingleOrDefault(u => u.Driver.Id == driver.Id);
                if (d != null)
                {
                    dDto.UserName = d.UserName;
                    dDto.UserPassword = d.DriverPassword;
                }
                return Ok(dDto);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [Route("api/Drivers/NotFired")]
        public IEnumerable<DriverDTO> GetNotFiredDrivers()
        {
            return ObjectMapper.Instance.Map<IList<Driver>, List<DriverDTO>>(mDriverService.GetAbleToOrdersDrivers());
        }

        [Route("api/Drivers/OnlineDrivers")]
        public IEnumerable<SimpleDriverReport> GetOnlineDrivers()
        {
            return mDriverService.GetOnlineDrivers();
        }

        [Route("api/Drivers/ActiveDrivers")]
        public IEnumerable<DriverDTO> GetActiveDrivers()
        {
            return ObjectMapper.Instance.Map<IList<Driver>, List<DriverDTO>>(mDriverService.GetActiveDrivers());
        }

        [Route("api/Drivers/FiredDrivers")]
        public IEnumerable<SimpleDriverReport> GetFiredDrivers()
        {
            return mDriverService.GetFiredDrivers();
        }

        [Route("api/Drivers/GetDriverByPhone")]
        public DriverDTO GetDriverByPhone(long phone)
        {
            return ObjectMapper.Instance.Map<Driver, DriverDTO>(mDriverService.GetDriverByPhone(phone));
        }

        [HttpPut]
        [Route("api/Drivers/BlockUnblockDriver")]
        [ValidateModel]
        public IHttpActionResult BlockUnblockDriver(DriverBlockDTO blockModel)
        {
            var userManager = Startup.UserManagerFactory();
            var user = userManager.Users.SingleOrDefault(u => u.UserName == blockModel.DispatcherLogin);

            if (user == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var changedDriver = mDriverService.BlockUnblockDriver(blockModel.DriverId, blockModel.Comment, user.Id,
                blockModel.Block);

            if (changedDriver == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Ok(ObjectMapper.Instance.Map<Driver, DriverDTO>(changedDriver));
        }

        [ValidateModel]
        public IHttpActionResult Post(DriverDTO driver)
        {
            if (mAccountService.GetUsers(UserManager).Any(u => u.UserName == driver.UserName))
            {
                return BadRequest("Водитель с таким логином уже существует");
            }

            var driverModel = ObjectMapper.Instance.Map<DriverDTO, Driver>(driver);
            var addedDriver = mDriverService.AddDriver(driverModel);

            try
            {
                UpdateDriverAccount(driver, addedDriver);
                addedDriver = mDriverService.GetDriver(addedDriver.Id);
            }
            catch
            {
                mDriverService.DeleteDriver(addedDriver.Id);
                throw;
            }
            return Ok(ObjectMapper.Instance.Map<Driver, DriverDTO>(addedDriver));
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, DriverDTO driver)
        {
            var existsDriver = mDriverService.GetDriver(id);
            if (existsDriver == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var driverModel = ObjectMapper.Instance.Map(driver, existsDriver);
            var updatedDriver = mDriverService.UpdateDriver(driverModel);
            try
            {
                UpdateDriverAccount(driver, updatedDriver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(ObjectMapper.Instance.Map<Driver, DriverDTO>(updatedDriver));
        }

        private void UpdateDriverAccount(DriverDTO driverDto, Driver driver)
        {
            var user = driver.Logins != null ? driver.Logins.FirstOrDefault() : null;
            if (user == null)
            {
                mAccountService.Register(driverDto.UserName, driverDto.UserPassword, driver.LastName, driver.FirstName,
                    driver.MiddleName, driver.Id, UserRole.Driver, null, null, null, UserManager);
            }
            else
            {
                var result = mAccountService.Update(driverDto.UserName, user.UserName, driverDto.UserPassword, driverDto.LastName,
                    driverDto.FirstName, driverDto.MiddleName, driver.Id, UserRole.Driver, null, null, null, UserManager);

                if (!result.Succeeded)
                {
                    var error = result.Errors.Aggregate("", (current, e) => current + e);
                    throw new Exception(error);
                }
            }
        }

        public void Delete(int id)
        {
            mDriverService.DeleteDriver(id);
        }

        [Route("api/Drivers/Light")]
        public IEnumerable<LightDriverReport> GetLightDrivers()
        {
            return mDriverService.GetLightDrivers();
        }

        [Route("api/Drivers/SimpleDriversList")]
        public IEnumerable<SimpleDriverReport> GetSimpleDriversList()
        {
            return mDriverService.GetSimpleDriversList();
        }

        [Route("api/Drivers/DriverCarReport")]
        public IEnumerable<DriverCarReport> GetDriverCarReport()
        {
            return mDriverService.GetDriverCarReport();
        }

        [Route("api/Drivers/AllLightDrivers")]
        public IEnumerable<DriverCarReport> GetAllLightDrivers()
        {
            return mDriverService.GetAllLightDrivers();
        }

        [HttpGet]
        [Route("api/Drivers/GetRating")]
        public IEnumerable<DriverScores> GetDriverRating(long driverId)
        {
            return mDriverService.GetDriverScores(driverId);
        }

        [HttpGet]
        [Route("api/Drivers/GetFeedback")]
        public IEnumerable<CustomerFeedback> GetCustomerFeedback(long driverId, DateTime fromDate, DateTime toDate)
        {
            return mDriverService.GetCustomerFeedbacks(driverId, fromDate, toDate);
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