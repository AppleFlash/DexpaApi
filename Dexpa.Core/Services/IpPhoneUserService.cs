using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Repositories;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public class IpPhoneUserService : IIpPhoneUserService
    {
        private IIpPhoneUserRepository mRepository;

        public IpPhoneUserService(IIpPhoneUserRepository repository)
        {
            mRepository = repository;
        }

        public IpPhoneUser GetUser(string id)
        {
            return mRepository.Single(u => u.UserId == id);
        }

        public IpPhoneUser AddUser(IpPhoneUser user)
        {
            user = mRepository.Add(user);
            mRepository.Commit();
            return user;
        }

        public IpPhoneUser UpdateUser(IpPhoneUser user)
        {
            user = mRepository.Update(user);
            mRepository.Commit();
            return user;
        }

        public void DeleteUser(string id)
        {
            var user = mRepository.Single(u => u.UserId == id);
            mRepository.Delete(user);
            mRepository.Commit();
        }

        public void Dispose()
        {
            mRepository.Dispose();
        }
    }
}
