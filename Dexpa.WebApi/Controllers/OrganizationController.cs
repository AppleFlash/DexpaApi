using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class OrganizationController : ApiControllerBase
    {
        private IOrganizationService mOrganizationService;

        private ITransactionService mTransactionService;

        public OrganizationController(IOrganizationService organizationService, ITransactionService transactionService)
        {
            mOrganizationService = organizationService;
            mTransactionService = transactionService;
        }

        public IEnumerable<LightOrganization> GetOrganizations()
        {
            //var organizations =  ObjectMapper.Instance.Map<IList<Organization>, List<OrganizationDTO>>(mOrganizationService.GetOrganizations());
            var organizations = mOrganizationService.GetOrganizations();
            for (int i = 0; i < organizations.Count; i++)
            {
                organizations[i].Balance = mTransactionService.GetOrganizationBalance(organizations[i].Id);
            }
            return organizations;
        }

        public OrganizationDTO GetOrganization(long id)
        {
            return ObjectMapper.Instance.Map<Organization, OrganizationDTO>(mOrganizationService.GetOrganization(id));
        }

        public HttpResponseMessage Post(OrganizationDTO organization)
        {
            var existsOrganization = mOrganizationService.GetOrganization(organization.Name);
            if (existsOrganization == null)
            {
                var organizationModel = ObjectMapper.Instance.Map<OrganizationDTO, Organization>(organization);
                var addOrganization = mOrganizationService.AddOrganization(organizationModel);
                return Request.CreateResponse(ObjectMapper.Instance.Map<Organization, OrganizationDTO>(addOrganization));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Такая организация уже есть");
            }
        }

        public IHttpActionResult Put(long id, OrganizationDTO organization)
        {
            var existsOrganization = mOrganizationService.GetOrganization(id);
            if (existsOrganization == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var updateOrganization = ObjectMapper.Instance.Map(organization, existsOrganization);
            mOrganizationService.UpdateOrganization(updateOrganization);
            return Ok(ObjectMapper.Instance.Map<Organization, OrganizationDTO>(updateOrganization));
        }

        public void DeleteOrganization(long id)
        {
            mOrganizationService.DeleteOrganization(id);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mOrganizationService.Dispose();
                mTransactionService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}