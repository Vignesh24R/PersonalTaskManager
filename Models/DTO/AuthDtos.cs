using System.ComponentModel.DataAnnotations;

namespace PersonalTaskManager.Models.DTO
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;  // input validation

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;  // plain text (hashed later)
    }

    public class LoginRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; } = string.Empty;  // login with either

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public int UserId { get; set; }      // for frontend
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;  // JWT for auth
    }

}
