using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Microsoft.AspNet.Identity;

namespace Dexpa.Core.Services
{
    public interface IAccountService : IDisposable
    {
        IdentityResult Register(string userName, string password, string lastName, string name, string middleName,
            long? driver, UserRole role, string ipUserName, string ipPassword, string ipProvider, UserManager<User> userManager);

        IdentityResult Update(string userName, string oldUserName, string password, string lastName, string name,
            string middleName, long? driver, UserRole role, string ipUserName, string ipPassword, string ipProvider, UserManager<User> userManager);

        List<User> GetUsers(UserManager<User> mUserManager);

        int GetUserRole(string username, UserManager<User> userManager);

        void Delete(string userName, UserManager<User> userManager);
    }
}