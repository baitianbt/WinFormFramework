using System.Reflection;
using WinFormFramework.Common.Attributes;

namespace WinFormFramework.Common.Security
{
    public static class PermissionChecker
    {
        public static bool HasRequiredPermissions(Type type, IPermissionValidator validator)
        {
            var attributes = type.GetCustomAttributes<RequirePermissionAttribute>(true);
            return CheckPermissions(attributes, validator);
        }

        public static bool HasRequiredPermissions(MethodInfo method, IPermissionValidator validator)
        {
            var attributes = method.GetCustomAttributes<RequirePermissionAttribute>(true);
            return CheckPermissions(attributes, validator);
        }

        private static bool CheckPermissions(IEnumerable<RequirePermissionAttribute> attributes, IPermissionValidator validator)
        {
            foreach (var attribute in attributes)
            {
                var permissionCodes = attribute.PermissionCode.Split(',');
                if (attribute.RequireAll)
                {
                    if (!validator.HasAllPermissions(permissionCodes))
                        return false;
                }
                else
                {
                    if (!validator.HasAnyPermission(permissionCodes))
                        return false;
                }
            }
            return true;
        }
    }
} 