using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Security;

namespace WinFormFramework.BLL.Security
{
    public class PermissionValidator : IPermissionValidator
    {
        private readonly IPermissionService _permissionService;
        private readonly int _userId;
        private HashSet<string>? _userPermissions;

        public PermissionValidator(IPermissionService permissionService, int userId)
        {
            _permissionService = permissionService;
            _userId = userId;
        }

        public bool HasPermission(string permissionCode)
        {
            return _permissionService.HasPermissionAsync(_userId, permissionCode).Result;
        }

        public bool HasAnyPermission(params string[] permissionCodes)
        {
            return _permissionService.HasAnyPermissionAsync(_userId, permissionCodes).Result;
        }

        public bool HasAllPermissions(params string[] permissionCodes)
        {
            return _permissionService.HasAllPermissionsAsync(_userId, permissionCodes).Result;
        }

        private async Task EnsurePermissionsLoadedAsync()
        {
            if (_userPermissions == null)
            {
                var permissions = await _permissionService.GetUserPermissionsAsync(_userId);
                _userPermissions = new HashSet<string>(
                    permissions.Select(p => p.Code),
                    StringComparer.OrdinalIgnoreCase);
            }
        }
    }
} 