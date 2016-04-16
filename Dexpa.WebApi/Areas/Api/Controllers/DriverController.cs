using System.Collections.Generic;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using ApiController = Dexpa.WebApi.Controllers.ApiController;

namespace Dexpa.WebApi.Areas.Api.Controllers
{
    public class DriverController : ApiController
    {
        private IDriverService mDriverService;

        public DriverController(IDriverService driverService)
        {
            mDriverService = driverService;
        }

        public Driver Get(int skip = 0, int take = DEFAULT_TAKE)
        {
            return null;
        }

        public IList<Driver> Drivers()
        {
            return mDriverService.GetDrivers();
        }
    }
}