using PersonalTaskManager.Models;
using PersonalTaskManager.Models.DTO;

namespace PersonalTaskManager.Services.AuthandToken
{
    public interface IAuthService
    {
        Task<bool> UsernameOrEmailExistsAsync(string username, string email);
        Task<User> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
