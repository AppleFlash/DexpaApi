using System.Web.Mvc;

namespace Dexpa.WebApi.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        protected const int DEFAULT_TAKE = 100;

        public ApiController()
        {
            Global.Inject(this);
        }
    }
}