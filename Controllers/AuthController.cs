using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Models.DTO;
using PersonalTaskManager.Services.AuthandToken;

namespace PersonalTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    //"email": "abc123@gmail.com",
    //"password": "12345678"
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]  // anyone can register
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);  // validation errors

            // Check uniqueness
            if (await _authService.UsernameOrEmailExistsAsync(request.Username, request.Email))
            {
                return Conflict(new { message = "Username or email already exists" });
            }

            var user = await _authService.RegisterAsync(request);  // creates + hashes

            return Ok(new { user.Id, user.Username, user.Email });  // no password!
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var auth = await _authService.LoginAsync(request);
            if (auth == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(auth);  // { userId, username, token }
        }

    }
}
