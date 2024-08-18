using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = SewingFactory.Models.Task;


namespace SewingFactory.Services.Service
{
    public class TaskService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;

        public TaskService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //Get all tasks
        public async Task<IEnumerable<Task>> GetAllTasks()
            => await _dbContext.Tasks.ToListAsync();

        //Get task by id
        public async Task<Task> GetTaskById(Guid id)
            => await _dbContext.Tasks.FindAsync(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

        //Create task, status, created date and deadline excluded
        public async Task<Task> CreateTask(TaskCreateDto dto)
        {
            //Inject data from dto to Task object
            var task = new Task
            {
                ID = Guid.NewGuid(),
                OrderID = dto.OrderID,
                Name = dto.Name,
                Description = dto.Description,
                Status = 0,
                CreatorID = dto.CreatorID,
                CreatedDate = DateTime.Now,
                Deadline = DateTime.Now.AddDays(1),
                GroupID = dto.GroupID,
            };
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<Task> UpdateTask(Guid id, TaskUpdateDto dto)
        {
            //Find existing task
            var task = await _dbContext.Tasks.FindAsync(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found."); ;

            // Update properties
            if (dto.OrderID != Guid.Empty)
            {
                task.OrderID = dto.OrderID;
            }
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                task.Name = dto.Name;
            }
            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                task.Description = dto.Description;
            }
            if (dto.Status.HasValue)
            {
                task.Status = dto.Status;
            }

            if (dto.Deadline.HasValue)
            {
                task.Deadline = dto.Deadline.Value;

            }
            if (dto.GroupID != Guid.Empty)
            {
                task.GroupID = dto.GroupID;
            }

            await _dbContext.SaveChangesAsync();

            return task;

        }

        public void DeleteTask(Guid id)
        {
            var task = _dbContext.Tasks.Find(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");
            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();
        }
    }


}
