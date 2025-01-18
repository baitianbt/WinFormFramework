using WinFormFramework.BLL.DTOs;

namespace WinFormFramework.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDTO?> GetByIdAsync(int id);
        Task<IEnumerable<RoleDTO>> GetAllAsync();
        Task<RoleDTO> CreateAsync(RoleDTO roleDto);
        Task UpdateAsync(RoleDTO roleDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoleDTO>> GetUserRolesAsync(int userId);
        Task AssignRolesToUserAsync(int userId, IEnumerable<int> roleIds);
    }
} 