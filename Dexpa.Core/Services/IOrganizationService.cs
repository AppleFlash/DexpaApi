using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;

namespace Dexpa.Core.Services
{
    public interface IOrganizationService : IDisposable
    {
        IList<LightOrganization> GetOrganizations();

        Organization GetOrganization(long id);

        Organization GetOrganization(string name);

        Organization AddOrganization(Organization organization);

        Organization UpdateOrganization(Organization organization);

        void DeleteOrganization(long id);
    }
}
