using AutoMapper;
using WinFormFramework.BLL.DTOs;
using WinFormFramework.BLL.Interfaces;
using WinFormFramework.Common.Logging;
using WinFormFramework.Common.Utils;
using WinFormFramework.DAL;
using WinFormFramework.DAL.Entities;

namespace WinFormFramework.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, ILogService logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting user by id: {id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.Error("Error getting all users", ex);
                throw;
            }
        }

        public async Task<UserDTO> CreateAsync(UserDTO userDto, string password)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.Password = PasswordHelper.HashPassword(password);
                
                var result = await _userRepository.AddAsync(user);
                _logger.Information($"Created user: {user.UserName}");
                
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error creating user: {userDto.UserName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(UserDTO userDto)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(userDto.Id);
                if (existingUser == null)
                    throw new KeyNotFoundException($"User with ID {userDto.Id} not found");

                _mapper.Map(userDto, existingUser);
                await _userRepository.UpdateAsync(existingUser);
                _logger.Information($"Updated user: {userDto.UserName}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating user: {userDto.UserName}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                _logger.Information($"Deleted user with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting user with ID: {id}", ex);
                throw;
            }
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                var user = await GetByUsernameAsync(username);
                if (user == null || !user.IsEnabled)
                    return false;

                var dbUser = await _userRepository.Query()
                    .FirstOrDefaultAsync(u => u.UserName == username);

                return dbUser != null && PasswordHelper.VerifyPassword(password, dbUser.Password);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error validating user: {username}", ex);
                throw;
            }
        }

        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _userRepository.Query()
                    .FirstOrDefaultAsync(u => u.UserName == username);
                
                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting user by username: {username}", ex);
                throw;
            }
        }
    }
} 