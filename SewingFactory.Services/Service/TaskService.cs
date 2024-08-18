using Microsoft.AspNetCore.Http.Features;
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
        public async Task<IEnumerable<TaskResponseDto>> GetAllTasks()
        {
            var tasks = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            var taskResponseDtos = tasks.Select(t => new TaskResponseDto
            {
                ID = t.ID,
                OrderID = t.OrderID,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                CreatorName = t.User?.Name,
                CreatedDate = t.CreatedDate,
                Deadline = t.Deadline,
                GroupName = t.Group?.Name

            });

            return taskResponseDtos;
        }

        //Get task by id
        public async Task<TaskResponseDto> GetTaskById(Guid id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            //define the dto from object
            var taskResponseDto = new TaskResponseDto
            {
                ID = task.ID,
                OrderID = task.OrderID,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                CreatorName = task.User?.Name,
                CreatedDate = task.CreatedDate,
                Deadline = task.Deadline,
                GroupName = task.Group?.Name
            };

            return taskResponseDto;

        }

        //Create task, status, created date and deadline excluded
        public async Task<TaskResponseDto> CreateTask(TaskCreateDto dto)
        {
            var order = await _dbContext.Orders.FindAsync(dto.OrderID) ?? throw new KeyNotFoundException($"Order with order ID not found.");
            var creator = await _dbContext.Users.FindAsync(dto.CreatorID) ?? throw new KeyNotFoundException($"User with creator ID not found.");

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
                GroupID = creator.GroupID,
            };
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            //Retrieve again to gain the relationships
            var getTask = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == task.ID);

            //define responsedto from the object
            var taskResponseDto = new TaskResponseDto
            {
                ID = getTask.ID,
                OrderID = getTask.OrderID,
                Name = getTask.Name,
                Description = getTask.Description,
                Status = getTask.Status,
                CreatorName = getTask.User?.Name,
                CreatedDate = getTask.CreatedDate,
                Deadline = getTask.Deadline,
                GroupName = getTask.Group?.Name
            };

            return taskResponseDto;
        }

        public async Task<TaskResponseDto> UpdateTask(Guid id, TaskUpdateDto dto)
        {
            //Find existing task
            var task = await _dbContext.Tasks.FindAsync(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            // Update properties
            if (dto.OrderID != Guid.Empty)
            {
                var order = await _dbContext.Orders.FindAsync(dto.OrderID) ?? throw new KeyNotFoundException($"Order with order ID not found.");
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

            await _dbContext.SaveChangesAsync();

            //Retrieve again to gain the relationships
            var getTask = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == task.ID);

            var taskResponseDto = new TaskResponseDto
            {
                ID = getTask.ID,
                OrderID = getTask.OrderID,
                Name = getTask.Name,
                Description = getTask.Description,
                Status = getTask.Status,
                CreatorName = getTask.User?.Name,
                CreatedDate = getTask.CreatedDate,
                Deadline = getTask.Deadline,
                GroupName = getTask.Group?.Name
            };

            return taskResponseDto;

        }

        public void DeleteTask(Guid id)
        {
            var task = _dbContext.Tasks.Find(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");
            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();
        }
    }


}
