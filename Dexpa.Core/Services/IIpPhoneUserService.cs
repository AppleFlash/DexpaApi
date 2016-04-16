using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IIpPhoneUserService : IDisposable
    {
        IpPhoneUser GetUser(string id);

        IpPhoneUser AddUser(IpPhoneUser user);

        IpPhoneUser UpdateUser(IpPhoneUser user);

        void DeleteUser(string id);
    }
}
