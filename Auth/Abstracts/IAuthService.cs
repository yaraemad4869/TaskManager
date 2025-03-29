using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Auth.Abstracts
{
    public interface IAuthService
    {
        Task<User> Register(UserDTO registerDto);
        Task<User> Login(LoginDTO loginDto);
        Task<User> GetCurrentUserAsync();
        string GenerateJwtToken(User user);
    }
}
