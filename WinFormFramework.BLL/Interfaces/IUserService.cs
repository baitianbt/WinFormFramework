using WinFormFramework.BLL.DTOs;

namespace WinFormFramework.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> CreateAsync(UserDTO userDto, string password);
        Task UpdateAsync(UserDTO userDto);
        Task DeleteAsync(int id);
        Task<bool> ValidateUserAsync(string username, string password);
        Task<UserDTO?> GetByUsernameAsync(string username);
    }
} 