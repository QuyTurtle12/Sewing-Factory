using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models.DTOs;
using SewingFactory.Services.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        private readonly ITokenService _tokenService;

        public TasksController(TaskService taskService, ITokenService tokenService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService)); ;
            _tokenService = tokenService;
        }

        // GET: api/Tasks
        [HttpGet]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "View task list of all groups"
        )]
        public async Task<IActionResult> GetTasks([Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)

        {
            try
            {
                return Ok(await _taskService.GetAll(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        // GET: api/Tasks
        [HttpGet("full")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "View task list of all groups (Non-pagination view)"
        )]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                return Ok(await _taskService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }


        // GET: api/Tasks/5
        [HttpGet("{id:guid}")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "View a specific task"
        )]
        public async Task<IActionResult> GetTask([Required] Guid id)
        {
            try
            {
                return Ok(await _taskService.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // PUT: api/Tasks/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Order")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager that created that task",
            Description = "Adjust corresponding order, task name, task description and deadline of the task (deadline must be in [yyyy-MM-dd HH:mm:ss] format)"
        )]
        public async Task<IActionResult> UpdateTaskInfo([Required] Guid id, [Required] TaskUpdateDto dto)
        {
            try
            {
                // Call the service to update the task
                return Ok(await _taskService.UpdateInfo(id, GetUserID(), dto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        //PATCH: api/Tasks/{id}/
        [HttpPatch("{id:guid}")]
        [Authorize(Policy = "Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Sewing Staff of that group",
            Description = "Update the current status (%) of the task"
        )]
        public async Task<IActionResult> UpdateTaskStatus([Required] Guid id, [Required] double? status)
        {
            try
            {
                return Ok(await _taskService.UpdateStatus(id, GetUserID(), status));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }


        // POST: api/Tasks
        [HttpPost]
        [Authorize(Policy = "Order")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager",
            Description = "Create a new task for the order, providing OrderID, GroupID, task name and description"
        )]
        public async Task<IActionResult> PostTask([Required] TaskCreateDto dto)
        {
            try
            {


                var task = await _taskService.Create(GetUserID(), dto);
                return CreatedAtAction(nameof(GetTask), new { id = task.ID }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        // DELETE: api/Tasks/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Order")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager that created that task",
            Description = "Delete the task if completed or neccessary"
        )]
        public IActionResult DeleteTask([Required] Guid id)
        {
            try
            {
                _taskService.Delete(id, GetUserID());
                return Ok("Task deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/order
        [HttpGet("search/order")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search tasks of an order"
        )]
        public async Task<IActionResult> SearchByOrderID([Required] Guid orderID, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByOrderID(orderID, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/name
        [HttpGet("search/name")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search tasks list with the name query, the query is included in the task name"
        )]
        public async Task<IActionResult> SearchByName([Required] string searchQuery, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByName(searchQuery, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/status
        [HttpGet("search/status")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search tasks list with the status, using min and max range to find (use the same values for both to find exactly)"
        )]
        public async Task<IActionResult> SearchByStatus([Required, Range(0, 1)] double min, [Required, Range(0, 1)] double max, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByStatus(min, max, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/creator
        [HttpGet("search/creator")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search tasks of a creator"
        )]
        public async Task<IActionResult> SearchByCreatorID([Required] Guid creatorID, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByCreatorID(creatorID, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/created-date
        [HttpGet("search/created-date")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search task list with the created date [yyyy-mm-dd], using the range to find tasks within (use the same values for both to find exactly)"
        )]
        public async Task<IActionResult> SearchByCreatedDate([Required] string startDate, [Required] string endDate, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByCreatedDate(startDate, endDate, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/deadline
        [HttpGet("search/deadline")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search task list with the deadline [yyyy-mm-dd], using the range to find tasks within (use the same values for both to find exactly)"
        )]
        public async Task<IActionResult> SearchByDeadline([Required] string startDate, [Required] string endDate, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByDeadline(startDate, endDate, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        // GET: api/Tasks/search/group
        [HttpGet("search/group")]
        [Authorize(Policy = "Order-Sewing")]
        [SwaggerOperation(
            Summary = "Authorization: Order Manager & Sewing Staff",
            Description = "Search tasks list with the group name query, must be exact name"
        )]
        public async Task<IActionResult> SearchByGroupName([Required] string groupName, [Required, Range(1, int.MaxValue)] int pageNumber = 1, [Required, Range(1, int.MaxValue)] int pageSize = 10)
        {
            try
            {
                return Ok(await _taskService.SearchByGroupName(groupName, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }


        private Guid GetUserID()
        {
            var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();

            // Decode the JWT token and extract claims
            var principal = _tokenService.DecodeJwtToken(token);
            var claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList();

            var userID = Guid.Empty;
            foreach (var claim in claims)
            {
                if (claim.Type == "userId")
                {
                    if (Guid.TryParse(claim.Value, out Guid parsedUserID))
                    {
                        userID = parsedUserID;
                    }
                    else
                    {
                        // Handle the case where the userId claim is not a valid Guid
                        throw new BadHttpRequestException("Invalid user ID format.");
                    }
                    break;
                }
            }

            return userID;
        }
    }
}
