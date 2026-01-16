using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalTaskManager.Models;
using PersonalTaskManager.Models.DTO;
using PersonalTaskManager.Services.AuthandToken;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalTaskManager.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user)
        {
            // Claims: data embedded in token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),     // subject = user ID
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),   // username
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),       // standard user ID claim
                new Claim(ClaimTypes.Name, user.Username)                       // display name
            };

            // Signing key (symmetric HMAC-SHA256)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,                    
                audience: _jwtSettings.Audience,                
                claims: claims,                                 // user data
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),  // expires in 60 min
                signingCredentials: creds                       // signature
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // JWT string
        }

    }
}
