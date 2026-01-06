using PersonalTaskManager.Models;

namespace PersonalTaskManager.Services.AuthandToken
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }

}
