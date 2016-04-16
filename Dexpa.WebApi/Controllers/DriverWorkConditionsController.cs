using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.DTO.Light;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class DriverWorkConditionsController : ApiControllerBase
    {
        private IDriverWorkConditionsService mWorkConditionsService;

        public DriverWorkConditionsController(IDriverWorkConditionsService workConditionsService)
        {
            mWorkConditionsService = workConditionsService;
        }

        public List<DriverWorkConditionsDTO> GetWorkConditions()
        {
            var conditions = mWorkConditionsService.GetWorkConditions();
            return ObjectMapper.Instance.Map<IList<DriverWorkConditions>, List<DriverWorkConditionsDTO>>(conditions);
        }

        [Route("api/DriverWorkConditions/Light")]
        [HttpGet]
        public List<WorkConditionLightDTO> GetLightWorkConditions()
        {
            var conditions = mWorkConditionsService.GetWorkConditions();
            return ObjectMapper.Instance.Map<IList<DriverWorkConditions>, List<WorkConditionLightDTO>>(conditions);
        }

        public IHttpActionResult GetWorkConditions(long id)
        {
            var conditions = mWorkConditionsService.GetWorkConditions(id);
            if (conditions != null)
            {
                return Ok(ObjectMapper.Instance.Map<DriverWorkConditions, DriverWorkConditionsDTO>(conditions));
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage Post(DriverWorkConditionsDTO conditions)
        {
            var conditionsModel = ObjectMapper.Instance.Map<DriverWorkConditionsDTO, DriverWorkConditions>(conditions);
            var addedConditions = mWorkConditionsService.AddWorkConditions(conditionsModel);
            if (addedConditions != null)
            {
                return Request.CreateResponse(ObjectMapper.Instance.Map<DriverWorkConditions, DriverWorkConditionsDTO>(addedConditions));
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Условие работы с таким названием уже существует");
        }

        public IHttpActionResult Put(long id, DriverWorkConditionsDTO conditions)
        {
            var existsConditions = mWorkConditionsService.GetWorkConditions(id);
            if (existsConditions == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var conditionsModel = ObjectMapper.Instance.Map(conditions, existsConditions);
            var updatedConditions = mWorkConditionsService.UpdateWorkConditions(conditionsModel);
            return Ok(ObjectMapper.Instance.Map<DriverWorkConditions, DriverWorkConditionsDTO>(updatedConditions));
        }

        public void Delete(int id)
        {
            mWorkConditionsService.DeleteWorkConditions(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mWorkConditionsService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}