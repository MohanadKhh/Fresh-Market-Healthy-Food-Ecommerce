using ECommerce.BLL;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            var result = await _authManager.RegisterUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var result = await _authManager.LoginUserAsync(loginDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
