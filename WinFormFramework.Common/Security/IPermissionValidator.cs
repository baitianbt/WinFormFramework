namespace WinFormFramework.Common.Security
{
    public interface IPermissionValidator
    {
        bool HasPermission(string permissionCode);
        bool HasAnyPermission(params string[] permissionCodes);
        bool HasAllPermissions(params string[] permissionCodes);
    }
} 