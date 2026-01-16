using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalTaskManager.Models;
using PersonalTaskManager.Models.DTO;
using PersonalTaskManager.Services.AuthandToken;

namespace PersonalTaskManager.Services.AuthandToken
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<bool> UsernameOrEmailExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            // Hash plain password using Microsoft's secure algorithm
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();  // Id auto-generated

            return user;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            // Find user by username OR email
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == request.UsernameOrEmail ||
                    u.Email == request.UsernameOrEmail);

            if (user == null) return null;  // invalid credentials

            // Verify hashed password matches plain input
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            // Generate JWT
            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Token = token
            };
        }

    }
}
