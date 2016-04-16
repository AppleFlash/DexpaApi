using System.Collections.Generic;

namespace Dexpa.Core.Model
{
    public static class Roles
    {
        private static Dictionary<UserRole, UserPermission> mRolesPermissions;

        static Roles()
        {
            mRolesPermissions = new Dictionary<UserRole, UserPermission>();
            mRolesPermissions.Add(UserRole.Admin, UserPermission.ViewEditAll);
        }

        public static UserPermission GetPermissionsByRole(string role)
        {
            return UserPermission.EditCars;
        }
    }
}
