using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class WayBillsController : ApiControllerBase
    {
        private IWayBillsService mWayBillsService;

        private IDriverService mDriverService;

        public WayBillsController(IWayBillsService wayBillsService, IDriverService driverService)
        {
            mWayBillsService = wayBillsService;
            mDriverService = driverService;
        }

        public IEnumerable<WayBillsDTO> GetWayBills(long? driverId = null, long? carId = null, DateTime? fromDate = null, DateTime? toDate = null, bool isActive = false)
        {
            List<WayBillsDTO> wayBills =
                ObjectMapper.Instance.Map<IList<WayBills>, List<WayBillsDTO>>(mWayBillsService.GetWayBillses(driverId, carId, fromDate, toDate, isActive));

            var drivers =
                mDriverService.GetDrivers()
                    .Select(
                        d =>
                            new Driver()
                            {
                                Id = d.Id,
                                Balance = d.Balance,
                                BalanceLimit = d.BalanceLimit,
                                DayTimeFee = d.DayTimeFee
                            });

            for (int i = 0; i < wayBills.Count; i++)
            {
                DriverDTO driver =
                    ObjectMapper.Instance.Map<Driver, DriverDTO>(drivers.Single(d => d.Id == wayBills[i].Driver.Id));

                var maxPeriod = (driver.Balance - driver.BalanceLimit)/driver.DayTimeFee;
                wayBills[i].MaxPeriod = maxPeriod;
                wayBills[i].Period = (wayBills[i].ToDate - wayBills[i].FromDate).TotalDays;
            }

            return wayBills;
        }

        public WayBillsDTO GetWayBills(long id)
        {
            WayBillsDTO wayBills = ObjectMapper.Instance.Map<WayBills, WayBillsDTO>(mWayBillsService.GetWayBills(id));
            DriverDTO driver =
                    ObjectMapper.Instance.Map<Driver, DriverDTO>(mDriverService.GetDriver(wayBills.Driver.Id));

            var maxPeriod = (driver.Balance - driver.BalanceLimit) / driver.DayTimeFee;
            wayBills.MaxPeriod = maxPeriod;
            wayBills.Period = (wayBills.ToDate - wayBills.FromDate).TotalDays;

            return wayBills;
        }

        [Route("api/waybills/driverwaybills")]
        public WayBillsDTO GetDriverWayBills(long driverId)
        {
            WayBillsDTO wayBills = ObjectMapper.Instance.Map<WayBills, WayBillsDTO>(mWayBillsService.GetDriverWayBills(driverId));

            return wayBills;
        }

        [Route("api/waybills/drivermaxperiod")]
        public double GetDriverMaxPeriod(long driverId)
        {
            DriverDTO driver = ObjectMapper.Instance.Map<Driver, DriverDTO>(mDriverService.GetDriver(driverId));
            var maxPeriod = (driver.Balance - driver.BalanceLimit) / driver.DayTimeFee;
            return maxPeriod;
        }

        [ValidateModel]
        public HttpResponseMessage Post(WayBillsDTO wayBills)
        {
            //var oldWayBills = GetDriverWayBills(wayBills.Driver.Id);
            //if (oldWayBills!=null && oldWayBills.EndMileage == 0)
            //{
            //    oldWayBills.EndMileage = oldWayBills.StartMileage;
            //    Put(oldWayBills.Id, oldWayBills);
            //}
            if (wayBills.FromDate < wayBills.ToDate &&
                (wayBills.ToDate - wayBills.FromDate).TotalDays <= wayBills.MaxPeriod)
            {
                var wayBillsModel = ObjectMapper.Instance.Map<WayBillsDTO, WayBills>(wayBills);
                wayBillsModel = mWayBillsService.AddWayBills(wayBillsModel);
                return Request.CreateResponse(ObjectMapper.Instance.Map<WayBills, WayBillsDTO>(wayBillsModel));   
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Неправильная дата");
            }
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, WayBillsDTO wayBills)
        {
            if (wayBills.FromDate < wayBills.ToDate &&
                (wayBills.ToDate - wayBills.FromDate).TotalDays <= wayBills.MaxPeriod)
            {
                var existsWayBills = ObjectMapper.Instance.Map<WayBillsDTO, WayBills>(wayBills);
                if (existsWayBills == null)
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
                var wayBillsModel = ObjectMapper.Instance.Map(wayBills, existsWayBills);
                var updatedWayBills = mWayBillsService.UpdateWayBills(wayBillsModel);
                return Ok(ObjectMapper.Instance.Map<WayBills, WayBillsDTO>(updatedWayBills));
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(long id)
        {
            mWayBillsService.DeleteWayBills(id);
        }
    }
}