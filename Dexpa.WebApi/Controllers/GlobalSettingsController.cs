using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class GlobalSettingsController : ApiControllerBase
    {
        private IGlobalSettingsService mGlobalSettingsService;

        public GlobalSettingsController(IGlobalSettingsService globalSettings)
        {
            mGlobalSettingsService = globalSettings;
        }

        public GlobalSettingsDTO GetSettings()
        {
            var settings = mGlobalSettingsService.GetSettings();
            return ObjectMapper.Instance.Map<GlobalSettings, GlobalSettingsDTO>(settings);
        }

        public IHttpActionResult Put(GlobalSettingsDTO settingsDto)
        {
            var oldSettings = mGlobalSettingsService.GetSettings();
            var newSettings = ObjectMapper.Instance.Map(settingsDto, oldSettings);

            mGlobalSettingsService.SaveSettings(newSettings);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mGlobalSettingsService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}