using Microsoft.AspNetCore.Authorization;
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

        // Constructor for dependency injection
        public AuthController(AuthService authService, ITokenService tokenService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token upon successful login.
        /// </summary>
        /// <param name="loginDto">The login data containing username and password.</param>
        /// <returns>ActionResult containing the JWT token or an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Login data is null");
            }
            try
            {
                // Authenticate user and generate token
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Return 401 Unauthorized if authentication fails
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Decodes a JWT token and returns its claims.
        /// </summary>
        /// <param name="token">The JWT token to decode.</param>
        /// <returns>ActionResult with the token claims or an error message.</returns>
        [HttpPost("decode")]
        public IActionResult DecodeToken([FromBody] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is null or empty");
            }
            try
            {
                // Decode the JWT token and extract claims
                var principal = _tokenService.DecodeJwtToken(token);
                var claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Ok(claims);
            }
            catch (SecurityTokenException ex)
            {
                // Return 400 Bad Request if token decoding fails
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
