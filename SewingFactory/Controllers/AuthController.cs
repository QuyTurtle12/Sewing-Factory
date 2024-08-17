using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SewingFactory.Services.Dto.UserDto.RequestDto;
using SewingFactory.Services.Service;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(AuthService authService, ITokenService tokenService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login data is null");
            }
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("decode")]
        public IActionResult DecodeToken([FromBody] string token)
        {
            try
            {
                var principal = _tokenService.DecodeJwtToken(token);
                var claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Ok(claims);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
