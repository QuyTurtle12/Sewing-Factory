using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Services.Dto.UserDto.RequestDto;
using SewingFactory.Services.Service;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        // Constructor for dependency injection
        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Creates a new user.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="request">The user creation request data.</param>
        /// <returns>ActionResult indicating success or failure.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateDto request)
        {
            if (request == null)
            {
                return BadRequest("User data is null");
            }
            try
            {
                var user = await _userService.CreateUserAsync(request);
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <returns>ActionResult with a list of users or an error message.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>ActionResult with the user details or an error message.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a user's information.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The update request data.</param>
        /// <returns>ActionResult with the updated user details or an error message.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateDto request)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, request);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status of a user.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user whose status is to be updated.</param>
        /// <param name="statusDto">The status update request data.</param>
        /// <returns>ActionResult with the updated user details or an error message.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateUserStatusDto statusDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid user ID");
            }

            try
            {
                var updatedUser = await _userService.UpdateUserStatusAsync(id, statusDto);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>ActionResult indicating success or failure.</returns>
        [Authorize(Policy = "Staff-Manager-Policy")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
