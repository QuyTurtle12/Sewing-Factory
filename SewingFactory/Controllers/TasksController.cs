using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto;
using SewingFactory.Services.Service;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService)); ;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                return Ok(await _taskService.GetAllTasks());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

        // GET: api/Tasks/5
        [HttpGet("{id:guid}")]
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
        public async Task<IActionResult> PutTask(Guid id, TaskUpdateDto dto)
        {
            try
            {
                // Call the service to update the task
                return Ok(await _taskService.UpdateTask(id, dto));
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
        public async Task<IActionResult> PostTask(TaskCreateDto dto)
        {
            try
            {
                var task = await _taskService.CreateTask(dto);
                return CreatedAtAction(nameof(GetTask), new { id = task.ID }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }

        // DELETE: api/Tasks/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return Ok("Task deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }

        }

    }
}
