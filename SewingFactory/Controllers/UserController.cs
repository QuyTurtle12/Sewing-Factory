using Microsoft.AspNetCore.Mvc;
using SewingFactory.Services.Dto.UserDto;
using SewingFactory.Services.Service;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDto request)
        {
            if (request == null)
            {
                return BadRequest("User data is null");
            }
            try
            {
                var user = _userService.CreateUser(request);
                return Ok("Create user successfully!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            try
            {
                var token = _userService.Login(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
