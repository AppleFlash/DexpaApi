using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class IpPhoneUserController : ApiControllerBase
    {
        private IIpPhoneUserService mService;

        public IpPhoneUserController(IIpPhoneUserService service)
        {
            mService = service;
        }

        public IpPhoneUserDTO GetUser(string id)
        {
            return ObjectMapper.Instance.Map<IpPhoneUser, IpPhoneUserDTO>(mService.GetUser(id));
        }

        [ValidateModel]
        public HttpResponseMessage Post(IpPhoneUserDTO user)
        {
            if (user.Login != null && user.Password != null && user.Realm != null)
            {
                user.Login = user.Login.Trim(' ');
                user.Password = user.Password.Trim(' ');
                user.Realm = user.Realm.Trim(' ');
            }
            var userModel = ObjectMapper.Instance.Map<IpPhoneUserDTO, IpPhoneUser>(user);
            var addedUser = mService.AddUser(userModel);
            return Request.CreateResponse(ObjectMapper.Instance.Map<IpPhoneUser, IpPhoneUserDTO>(addedUser));
        }

        [ValidateModel]
        public IHttpActionResult Put(string id, IpPhoneUserDTO user)
        {
            var existsUser = mService.GetUser(id);
            if (existsUser == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            if (user.Login != null && user.Password != null && user.Realm != null)
            {
                user.Login = user.Login.Trim(' ');
                user.Password = user.Password.Trim(' ');
                user.Realm = user.Realm.Trim(' ');
            }
            var userModel = ObjectMapper.Instance.Map(user, existsUser);
            var updatedUser = mService.UpdateUser(userModel);
            return Ok(ObjectMapper.Instance.Map<IpPhoneUser, IpPhoneUserDTO>(updatedUser));
        }

        public void Delete(string id)
        {
            mService.DeleteUser(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}