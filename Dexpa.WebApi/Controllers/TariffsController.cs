using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.DTO.Light;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class TariffsController : ApiControllerBase
    {
        private ITariffsService mTariffsService;

        public TariffsController(ITariffsService tariffsService)
        {
            mTariffsService = tariffsService;
        }

        public IEnumerable<TariffDTO> GetTariffs()
        {
            var tariffs = mTariffsService.GetTariffs();
            return ObjectMapper.Instance.Map<IList<Tariff>, List<TariffDTO>>(tariffs);
        }

        [Route("api/Tariffs/Light")]
        public List<LightTariffDTO> GetLightTariffs()
        {
            var tariffs = mTariffsService.GetLightTariff();
            return ObjectMapper.Instance.Map<IList<LightTariff>, List<LightTariffDTO>>(tariffs);
        }

        public IHttpActionResult GetTariff(long id)
        {
            var tarif = mTariffsService.GetTariff(id);
            if (tarif != null)
            {
                return Ok(ObjectMapper.Instance.Map<Tariff, TariffDTO>(tarif));
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [ValidateModel]
        public HttpResponseMessage Post(TariffDTO tariffDto)
        {
            Tariff tariff = new Tariff();
            ObjectMapper.Instance.Map(tariffDto, tariff);
            var addTariff = mTariffsService.AddTarif(tariff);

            if (addTariff != null)
            {
                return Request.CreateResponse(ObjectMapper.Instance.Map<Tariff, TariffDTO>(addTariff));
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Тариф с таким названием или сокращением уже существует");
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, TariffDTO tariffDto)
        {
            var existsTariff = mTariffsService.GetTariff(id);
            if (existsTariff == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var tariffModel = ObjectMapper.Instance.Map(tariffDto, existsTariff);
            var updatedTariff = mTariffsService.UpdateTarif(tariffModel);
            return Ok(ObjectMapper.Instance.Map<Tariff, TariffDTO>(updatedTariff));
        }

        public void Delete(long id)
        {
            mTariffsService.DeleteTarif(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mTariffsService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}