using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Services.DTOs.UserDto.RequestDto;
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
        [Authorize(Policy = "Staff")]
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
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// <para>Requires the caller to have the Staff-Manager-Policy authorization.</para>
        /// </summary>
        /// <returns>ActionResult with a list of users or an error message.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("listed")]
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
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of users based on optional filters for name, status, role name, group name, role ID, and group ID.
        /// </summary>
        /// <param name="name">The name to filter by (optional).</param>
        /// <param name="username">The username to filter by (optional).</param>
        /// <param name="status">The status to filter by (optional).</param>
        /// <param name="roleName">The role name to filter by (optional).</param>
        /// <param name="groupName">The group name to filter by (optional).</param>
        /// <param name="roleId">The role ID to filter by (optional).</param>
        /// <param name="groupId">The group ID to filter by (optional).</param>
        /// <param name="minSalary">The minimum salary to filter by (optional).</param>
        /// <param name="maxSalary">The maximum salary to filter by (optional).</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of UserDto objects and the total count of users.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("search")]
        public async Task<ActionResult> SearchUsers(
            [FromQuery] string? name,
            [FromQuery] string? username,
            [FromQuery] bool? status,
            [FromQuery] string? roleName,
            [FromQuery] string? groupName,
            [FromQuery] Guid? roleId,
            [FromQuery] Guid? groupId,
            [FromQuery] double? minSalary,
            [FromQuery] double? maxSalary,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            var (users, totalCount) = await _userService.SearchUsersAsync(
                name, username, status, roleName, groupName, roleId, groupId, minSalary, maxSalary, pageNumber, pageSize);

            return Ok(new
            {
                Users = users,
                TotalCount = totalCount
            });
        }

        /// <summary>
        /// Retrieves a paginated list of users.
        /// <para>This endpoint requires the "Staff-Manager-Policy" authorization policy.</para>
        /// <para>Pagination parameters are provided in the query string, with default values of page number 1 and page size 10.</para>
        /// <para>Returns a JSON object containing the total count of users, the current page number, the page size, and the list of users for the current page.</para>
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of users per page. Defaults to 10.</param>
        /// <returns>An IActionResult containing the paginated user data, or an error message if the request is invalid or fails.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                var (users, totalCount) = await _userService.GetPagedUsersAsync(pageNumber, pageSize);

                var result = new
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Users = users
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"{ex.Message}");
            }
        }




        /// <summary>
        /// Retrieves a user by their ID.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>ActionResult with the user details or an error message.</returns>
        [Authorize(Policy = "Staff")]
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
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates a user's information.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The update request data.</param>
        /// <returns>ActionResult with the updated user details or an error message.</returns>
        [Authorize(Policy = "Staff")]
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
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status of a user.
        /// Requires the caller to have the Staff-Manager-Policy authorization.
        /// </summary>
        /// <param name="id">The ID of the user whose status is to be updated.</param>
        /// <param name="statusDto">The status update request data.</param>
        /// <returns>ActionResult with the updated user details or an error message.</returns>
        [Authorize(Policy = "Staff")]
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
                return StatusCode(500, $"{ex.Message}");
            }
        }

        ///// <summary>
        ///// Deletes a user by their ID.
        ///// Requires the caller to have the Staff-Manager-Policy authorization.
        ///// </summary>
        ///// <param name="id">The ID of the user to delete.</param>
        ///// <returns>ActionResult indicating success or failure.</returns>
        //[Authorize(Policy = "Staff-Manager-Policy")]
        //[HttpDelete]
        //[Route("{id:guid}")]
        //public async Task<IActionResult> DeleteUser(Guid id)
        //{
        //    try
        //    {
        //        await _userService.DeleteUserAsync(id);
        //        return Ok("User deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed
        //        return StatusCode(500, $"{ex.Message}");
        //    }
        //}

        /// <summary>
        /// Changes the password for a staff member.
        /// </summary>
        /// <param name="id">The ID of the user whose password is to be changed.</param>
        /// <param name="request">The password change request data.</param>
        /// <returns>ActionResult indicating success or failure of the password change.</returns>
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordForStaffDto request)
        {
            if (request == null)
            {
                return BadRequest("Password change data is null.");
            }

            try
            {
                var result = await _userService.ChangePasswordForStaff(id, request);

                if (result.Success)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of users filtered by roleId and groupId.
        /// If roleId or groupId are null, the method will not filter by those parameters.
        /// </summary>
        /// <param name="roleId">The role ID to filter by. If null, no role filter is applied.</param>
        /// <param name="groupId">The group ID to filter by. If null, no group filter is applied.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of users and the total count of users.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("search/role-group-id")]
        public async Task<IActionResult> GetUsersByRoleAndGroup(Guid? roleId = null, Guid? groupId = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                var (users, totalCount) = await _userService.GetUsersByRoleAndGroupAsync(roleId, groupId, pageNumber, pageSize);
                var result = new { TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize, Users = users };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of users filtered by roleName and groupName.
        /// If roleName or groupName are null, the method will not filter by those parameters.
        /// </summary>
        /// <param name="roleName">The role name to filter by. If null, no role filter is applied.</param>
        /// <param name="groupName">The group name to filter by. If null, no group filter is applied.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of users and the total count of users.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("search/role-group-name")]
        public async Task<IActionResult> GetUsersByRoleAndGroupName(string? roleName, string? groupName, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                var (users, totalCount) = await _userService.GetUsersByRoleAndGroupNameAsync(roleName, groupName, pageNumber, pageSize);
                var result = new { TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize, Users = users };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of users filtered by name.
        /// </summary>
        /// <param name="name">The name to filter by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of users and the total count of users.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("search/name")]
        public async Task<IActionResult> GetUsersByName(string? name, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                var (users, totalCount) = await _userService.GetUsersByNameAsync(name, pageNumber, pageSize);
                var result = new { TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize, Users = users };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of users filtered by status.
        /// </summary>
        /// <param name="status">The status to filter by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of users and the total count of users.</returns>
        [Authorize(Policy = "Staff")]
        [HttpGet("search/status")]
        public async Task<IActionResult> GetUsersByStatus(bool? status = true, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100.");
                }

                bool statusValue = status ?? true;

                var (users, totalCount) = await _userService.GetUsersByStatusAsync(statusValue, pageNumber, pageSize);
                var result = new { TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize, Users = users };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        

    }
}
