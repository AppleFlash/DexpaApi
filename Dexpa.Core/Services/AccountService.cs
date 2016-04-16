using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Microsoft.AspNet.Identity;
using NLog;

namespace Dexpa.Core.Services
{
    public class AccountService : IAccountService
    {
        private IIpPhoneUserService mIpPhoneUserService;

        private Logger mLogger = LogManager.GetCurrentClassLogger();

        public AccountService(IIpPhoneUserService ipPhoneUserService)
        {
            mIpPhoneUserService = ipPhoneUserService;
        }

        public IdentityResult Register(string userName, string password, string lastName, string name,
            string middleName, long? driverId, UserRole role, string ipUserName, string ipPassword, string ipProvider, UserManager<User> mUserManager)
        {
            var user = new User
            {
                UserName = userName,
                DriverId = driverId,
                Role = role,
                DriverPassword = role == UserRole.Driver ? password : null,
                LastName = lastName,
                Name = name,
                MiddleName = middleName
            };

            var identityResult = mUserManager.Create(user, password);

            if (identityResult.Succeeded)
            {
                var ipPhoneUser = mIpPhoneUserService.GetUser(user.Id);
                if (ipPhoneUser != null)
                {
                    mIpPhoneUserService.DeleteUser(ipPhoneUser.UserId);
                }

                mIpPhoneUserService.AddUser(new IpPhoneUser
                {
                    Login = ipUserName,
                    Password = ipPassword,
                    Realm = ipProvider,
                    UserId = user.Id
                });
            }
            else
            {
                mLogger.Error(identityResult.Errors.Aggregate((e1, e2) => e1 + ", " + e2));
            }

            return identityResult;
        }

        public IdentityResult Update(string userName, string oldUserName, string password, string lastName, string name,
            string middleName, long? driverId, UserRole role, string ipUserName, string ipPassword, string ipProvider, UserManager<User> mUserManager)
        {
            var user = mUserManager.Users.SingleOrDefault(u => u.UserName == oldUserName);

            if (user == null)
            {
                return IdentityResult.Failed("Пользователь не существует");
            }

            user.UserName = userName;

            user.LastName = lastName;
            user.Name = name;
            user.MiddleName = middleName;
            user.Role = role;

            if (password != null && user.Role == UserRole.Driver)
            {
                user.DriverPassword = password;
            }

            IdentityResult result = mUserManager.Update(user);

            if (!result.Succeeded)
            {
                return result;
            }

            if (password != null)
            {
                var res = mUserManager.RemovePassword(user.Id);
                var setRes = mUserManager.AddPassword(user.Id, password);
                if (!setRes.Succeeded)
                {
                    return setRes;
                }
            }

            var ipPhoneUser = mIpPhoneUserService.GetUser(user.Id);
            if (ipPhoneUser != null)
            {
                mIpPhoneUserService.DeleteUser(ipPhoneUser.UserId);
            }

            mIpPhoneUserService.AddUser(new IpPhoneUser
            {
                Login = ipUserName,
                Password = ipPassword,
                Realm = ipProvider,
                UserId = user.Id
            });

            return result;
        }

        public List<User> GetUsers(UserManager<User> mUserManager)
        {
            return mUserManager.Users.ToList();
        }

        public int GetUserRole(string username, UserManager<User> mUserManager)
        {
            var user = mUserManager.Users.SingleOrDefault(u => u.UserName == username);
            if (user != null)
            {
                return (int)user.Role;
            }
            else
            {
                return 999;
            }
        }

        public void Delete(string userName, UserManager<User> mUserManager)
        {
            var user = mUserManager.FindByName(userName);
            var result = mUserManager.Delete(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                throw new InvalidOperationException(error);
            }
        }

        public void Dispose()
        {

        }
    }
}
