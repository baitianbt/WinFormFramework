namespace WinFormFramework.BLL.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<IEnumerable<PermissionDTO>> GetRolePermissionsAsync(int roleId);
        Task<IEnumerable<PermissionDTO>> GetUserPermissionsAsync(int userId);
        Task AssignPermissionsToRoleAsync(int roleId, IEnumerable<int> permissionIds);
        Task<bool> HasPermissionAsync(int userId, string permissionCode);
        Task<bool> HasAnyPermissionAsync(int userId, params string[] permissionCodes);
        Task<bool> HasAllPermissionsAsync(int userId, params string[] permissionCodes);
    }
} 