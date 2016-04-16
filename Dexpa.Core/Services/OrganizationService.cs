using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private IOrganizationRepository mRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            mRepository = organizationRepository;
        }

        public IList<LightOrganization> GetOrganizations()
        {
            var organizations = mRepository.List();

            List<LightOrganization> report = new List<LightOrganization>();

            foreach (var organization in organizations)
            {
                report.Add(new LightOrganization()
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    DateFrom = organization.DateFrom,
                    DateTo = organization.DateTo,
                    TariffName = organization.Tariff.Name,
                    Codeword = organization.Codeword
                }); 
            }

            return report;
        }

        public Organization GetOrganization(long id)
        {
            return mRepository.Single(o => o.Id == id);
        }

        public Organization GetOrganization(string name)
        {
            return mRepository.Single(o => o.Name == name);
        }

        public Organization AddOrganization(Organization organization)
        {
            organization = mRepository.Add(organization);
            mRepository.Commit();
            return organization;
        }

        public Organization UpdateOrganization(Organization organization)
        {
            organization = mRepository.Update(organization);
            mRepository.Commit();
            return organization;
        }

        public void DeleteOrganization(long id)
        {
            var organization = mRepository.Single(o => o.Id == id);
            if (organization != null)
            {
                mRepository.Delete(organization);
                mRepository.Commit();
            }
        }

        public void Dispose()
        {
            mRepository.Dispose();
        }
    }
}
