using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class RegionsController : ApiControllerBase
    {
        private IRegionService mRegionsService;

        public RegionsController(IRegionService regionService)
        {
            mRegionsService = regionService;
        }

        public IEnumerable<RegionDTO> GetRegions()
        {
            var regions = mRegionsService.GetRegions();
            return ObjectMapper.Instance.Map<IList<Region>, List<RegionDTO>>(regions);
        }

        public IHttpActionResult GetRegions(long id)
        {
            var region = mRegionsService.GetRegion(id);
            if (region != null)
            {
                return Ok(ObjectMapper.Instance.Map<Region, RegionDTO>(region));
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mRegionsService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}