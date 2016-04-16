using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;
using Microsoft.AspNet.Identity;

namespace Dexpa.WebApi.Controllers
{
    public class RepairsController : ApiControllerBase
    {
        private IRepairService mRepairService;

        public RepairsController(IRepairService repairService)
        {
            mRepairService = repairService;
        }

        public IHttpActionResult GetRepair(long id)
        {
            var repair = mRepairService.GetRepair(id);
            if (repair != null)
            {
                var rDto = ObjectMapper.Instance.Map<Repair, RepairDTO>(repair);
                return Ok(rDto);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [ValidateModel]
        public IHttpActionResult Post(RepairDTO repair)
        {
            var repairModel = ObjectMapper.Instance.Map<RepairDTO, Repair>(repair);

            var userManager = Startup.UserManagerFactory();
            var user = userManager.Users.SingleOrDefault(u => u.UserName == repair.ImplementedByLogin);

            if (user == null)
                return StatusCode(HttpStatusCode.BadRequest);

            repairModel.ImplementedById = user.Id;
            
            var addedRepair = mRepairService.AddRepair(repairModel);
            return Ok(ObjectMapper.Instance.Map<Repair, RepairDTO>(addedRepair));
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, RepairDTO repair)
        {
            var existsRepair = mRepairService.GetRepair(id);
            if (existsRepair == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            var userManager = Startup.UserManagerFactory();
            var user = userManager.Users.SingleOrDefault(u => u.UserName == repair.ImplementedByLogin);
            if (user == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var repairModel = ObjectMapper.Instance.Map(repair, existsRepair);

            repairModel.ImplementedById = user.Id;

            var updatedRepair = mRepairService.UpdateRepair(repairModel);
            return Ok(ObjectMapper.Instance.Map<Repair, RepairDTO>(updatedRepair));
        }

        public void Delete(int id)
        {
            mRepairService.DeleteRepair(id);
        }

        [HttpGet]
        public IEnumerable<RepairDTO> CarRepairsReport(long carId)
        {
            return
                ObjectMapper.Instance.Map<IList<Repair>, List<RepairDTO>>(
                    mRepairService.GetCarRepairs(carId));
        }
    }
}