using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Dexpa.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiControllerBase
    {
        private IAccountService mAccountService;

        public UserManager<User> UserManager { get; private set; }

        public AccountController(IAccountService accountService)
        {
            mAccountService = accountService;
            UserManager = Startup.UserManagerFactory();
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegisterUpdateUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                /*long? driverId = null;
                if (!long.TryParse(model.DriverId, out driverId))
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }*/

                var result = mAccountService.Register(model.UserName, model.Password,
                    model.LastName, model.Name, model.MiddleName,
                    model.DriverId, model.Role,
                    model.IpUserName, model.Password, model.IpProvider,
                    UserManager);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

                return Ok(model.DriverId);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [Route("Update")]
        public IHttpActionResult Update(RegisterUpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*long driverId;
            if (!long.TryParse(model.DriverId, out driverId))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }*/

            var result = mAccountService.Update(model.UserName, model.OldUserName, model.Password,
                model.LastName, model.Name, model.MiddleName,
                model.DriverId, model.Role, 
                model.IpUserName, model.IpPassword, model.IpProvider, UserManager);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Delete")]
        public IHttpActionResult Delete(string userName)
        {
            mAccountService.Delete(userName, UserManager);
            return Ok();
        }

        [HttpGet]
        [Route("GetUserRole")]
        public string GetUserRole(string username)
        {
            var userRoleNumber = mAccountService.GetUserRole(username, UserManager);
            switch (userRoleNumber)
            {
                case 0:
                    return "Администратор";
                case 1:
                    return "Водитель";
                case 2:
                    return "Диспетчер";
            }
            return null;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}