using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Dexpa.Core.Model;
using Dexpa.WebApi.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;

namespace Dexpa.WebApi.Controllers
{
    [HandleError]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.Authorize]
    public class ApiControllerBase : ApiController
    {
        protected const int DEFAULT_TAKE = 100;

        protected Logger mLogger = LogManager.GetCurrentClassLogger();

        protected User UserAccount
        {
            get
            {
                if (User == null)
                {
                    return null;
                }
                var userId = User.Identity.GetUserId();
                var userAccount = mUserManager.Users.FirstOrDefault(u => u.Id == userId);
                if (userAccount != null)
                {
                    //var roles = System.Web.Security.Roles.GetRolesForUser(userAccount.UserName);
                    // userAccount.Permissions = Roles.GetPermissionsByRole(roles.FirstOrDefault());
                }

                return userAccount;
            }
        }

        protected IdentityDbContext<User> mUserManager;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            mUserManager = NinjectWebCommon.Resolve<IdentityDbContext<User>>();
        }

        protected void LogRequest()
        {
            mLogger.Debug("Request: {0}, Method: {1}, Headers: {2}, Content: {3}",
                Request.RequestUri,
                Request.Method,
                Request.Headers,
                Request.Content.ReadAsStringAsync().Result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mUserManager.Dispose();
            }

            base.Dispose(disposing);
        }
        /*
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null)
            {
                //Logger.LogError("Unhandled exception: " + filterContext.Exception, filterContext.Exception);

                if (filterContext.HttpContext.Request.ParameterInHeader(Configuration.RequestTypeParameterName) &&
                    filterContext.HttpContext.Request.GetParameterValueFromHeader(Configuration.RequestTypeParameterName)
                        .ToLower() == Configuration.ApiParameter.ToLower())
                {
                    // API call
                    if (filterContext.Exception is ApiServerException)
                    {
                        var apiException = filterContext.Exception as ApiServerException;
                        filterContext.Result = new ApiHttpStatusCodeResult(HttpStatusCode.InternalServerError,
                            apiException.Message);
                    }
                    else
                        filterContext.Result = new ApiHttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    filterContext.ExceptionHandled = true;
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = RedirectToAction("General", "Error", new {area = ""});
                    base.OnException(filterContext);
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }*/
    }
}