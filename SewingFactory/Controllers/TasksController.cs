using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto;
using SewingFactory.Services.Service;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
        public async Task<IActionResult> GetTasks(int page, int pageSize)
        {
            try
            {
                return Ok(await _taskService.GetAllTasks(page, pageSize));
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
        public async Task<IActionResult> GetTask(Guid id)
        {
            try
            {
                return Ok(await _taskService.GetTaskById(id));
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
            Description = "Adjust corresponding order, task name, task description and deadline of the task"
        )]
        public async Task<IActionResult> UpdateTaskInfo(Guid id, TaskUpdateDto dto)
        {
            try
            {
                // Call the service to update the task
                return Ok(await _taskService.UpdateTaskInfo(id, GetUserID(), dto));
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
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [Required] double? status)
        {
            try
            {
                return Ok(await _taskService.UpdateTaskStatus(id, GetUserID(), status));
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
        public async Task<IActionResult> PostTask(TaskCreateDto dto)
        {
            try
            {


                var task = await _taskService.CreateTask(GetUserID(), dto);
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
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                _taskService.DeleteTask(id, GetUserID());
                return Ok("Task deleted");
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
