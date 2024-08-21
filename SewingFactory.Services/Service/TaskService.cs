using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Core;
using Task = SewingFactory.Models.Task;
using SewingFactory.Models.DTOs;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System;


namespace SewingFactory.Services.Service
{
    public class TaskService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;

        public TaskService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        //Get all tasks
        public async Task<IEnumerable<TaskViewDto>> GetAll(int pageNumber, int pageSize)
        {
            var tasks = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();
        }

        //Get all tasks (No-pagination view)
        public async Task<IEnumerable<TaskViewDto>> GetAll()
        {

            var tasks = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            return taskViewDtos;
        }

        //Get task by id
        public async Task<TaskViewDto> GetById(Guid id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            //define the dto from object
            var taskViewDto = GetMapper(task);

            return taskViewDto;

        }

        //Create task, status, created date and deadline excluded
        public async Task<TaskViewDto> Create(Guid creatorID, TaskCreateDto dto)
        {
            var order = await _dbContext.Orders.FindAsync(dto.OrderID) ?? throw new KeyNotFoundException($"Order with order ID not found.");
            if (dto.GroupID == Guid.Empty) throw new KeyNotFoundException($"Group with group ID invalid.");
            var group = await _dbContext.Groups.FindAsync(dto.GroupID) ?? throw new KeyNotFoundException($"Group with group ID not found.");
            var creator = await _dbContext.Users.FindAsync(creatorID) ?? throw new KeyNotFoundException($"User with creator ID not found.");

            //Inject data from dto to Task object
            var task = new Task
            {
                ID = Guid.NewGuid(),
                OrderID = dto.OrderID,
                Name = dto.Name,
                Description = dto.Description,
                Status = 0,
                CreatorID = creatorID,
                CreatedDate = DateTime.Now,
                Deadline = DateTime.Now.AddDays(1),
                GroupID = dto.GroupID,
            };
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            //Retrieve again to gain the relationships
            var getTask = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == task.ID);

            //define ViewDto from the object
            var taskViewDto = GetMapper(task);

            return taskViewDto;
        }

        //Update task information, except status
        public async Task<TaskViewDto> UpdateInfo(Guid id, Guid creatorID, TaskUpdateDto dto)
        {
            //Find existing task
            var task = await _dbContext.Tasks.FindAsync(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            //Check if the creatorID matches with the creator ID in database
            if (creatorID != task.CreatorID) throw new UnauthorizedAccessException("Unauthorized action detected");

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

            if (!string.IsNullOrWhiteSpace(dto.Deadline))
            {
                DateTime deadline;
                bool success = DateTime.TryParseExact(dto.Deadline, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadline);
                if (!success) throw new Exception("Incorrect deadline format yyyy-MM-dd HH:mm:ss");

                if (deadline < task.CreatedDate) throw new Exception("Deadline should be set later than created date");
                task.Deadline = deadline;
            }

            await _dbContext.SaveChangesAsync();

            //Retrieve again to gain the relationships
            var getTask = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == task.ID);

            var taskViewDto = GetMapper(getTask);

            return taskViewDto;

        }

        //Update task status
        public async Task<TaskViewDto> UpdateStatus(Guid id, Guid staffID, double? status)
        {
            //Find existing task
            var task = await _dbContext.Tasks.Include(t => t.Group).FirstOrDefaultAsync(t => t.ID == id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            //Find the staff who is calling the method
            var staff = await _dbContext.Users.FindAsync(staffID);

            //Check if the staff is on the team as the task distributed into
            if (staff?.GroupID != task.GroupID) throw new UnauthorizedAccessException("Unauthorized action detected");

            if (status.HasValue)
            {
                if (status < 0 || status > 1) throw new Exception("Task status must be in range 0 - 1");

                task.Status = status;
            }
            else throw new ArgumentNullException("Status input null");

            await _dbContext.SaveChangesAsync();

            //Retrieve again to gain the relationships
            var getTask = await _dbContext.Tasks
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ID == task.ID);

            var taskViewDto = GetMapper(getTask);

            return taskViewDto;
        }

        public void Delete(Guid id, Guid creatorID)
        {
            var task = _dbContext.Tasks.Find(id) ?? throw new KeyNotFoundException($"Task with ID '{id}' not found.");

            //Check if the creatorID matches with the creator ID in database
            if (creatorID != task.CreatorID) throw new UnauthorizedAccessException("Unauthorized action detected");

            _dbContext.Tasks.Remove(task);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<TaskViewDto>> SearchByOrderID(Guid orderID, int pageNumber, int pageSize)
        {
            var order = await _dbContext.Orders.FindAsync(orderID) ?? throw new KeyNotFoundException($"Order with ID '{orderID}' not found.");

            var tasks = await _dbContext.Tasks
                .Where(t => t.OrderID == orderID)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();


            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();

        }

        public async Task<IEnumerable<TaskViewDto>> SearchByName(string? searchQuery, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(searchQuery)) throw new ArgumentNullException("Search query null");

            var tasks = await _dbContext.Tasks
                .Where(t => t.Name.ToLower().Trim().Contains(searchQuery.ToLower().Trim()))
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();
        }

        public async Task<IEnumerable<TaskViewDto>> SearchByStatus(double min, double max, int pageNumber, int pageSize)
        {
            if (min > max) throw new Exception("Min value is higher than max value");

            var tasks = await _dbContext.Tasks
                .Where(t => t.Status >= min && t.Status <= max)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();

        }

        public async Task<IEnumerable<TaskViewDto>> SearchByCreatorID(Guid creatorID, int pageNumber, int pageSize)
        {
            var user = await _dbContext.Users.FindAsync(creatorID) ?? throw new KeyNotFoundException($"Creator with ID '{creatorID}' not found.");

            var tasks = await _dbContext.Tasks
                .Where(t => t.CreatorID == creatorID)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();


            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();

        }

        public async Task<IEnumerable<TaskViewDto>> SearchByCreatedDate(string min, string max, int pageNumber, int pageSize)
        {
            DateOnly startDateOnly;
            bool success = DateOnly.TryParseExact(min, "yyyy-MM-dd", out startDateOnly); // Try to parse the string to DateOnly

            if (!success) throw new Exception("Incorrect start date format yyyy-mm-dd");

            DateOnly endDateOnly;
            success = DateOnly.TryParseExact(max, "yyyy-MM-dd", out endDateOnly);

            if (!success) throw new Exception("Incorrect end date format yyyy-mm-dd");

            if (startDateOnly > endDateOnly) throw new Exception("Start date should not be later than end date");

            DateTime startDate = startDateOnly.ToDateTime(TimeOnly.MinValue);
            DateTime endDate = endDateOnly.ToDateTime(TimeOnly.MaxValue);

            var tasks = await _dbContext.Tasks
                .Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();

        }

        public async Task<IEnumerable<TaskViewDto>> SearchByDeadline(string min, string max, int pageNumber, int pageSize)
        {
            DateOnly startDateOnly;
            bool success = DateOnly.TryParseExact(min, "yyyy-MM-dd", out startDateOnly); // Try to parse the string to DateOnly

            if (!success) throw new Exception("Incorrect start date format yyyy-MM-dd");

            DateOnly endDateOnly;
            success = DateOnly.TryParseExact(max, "yyyy-MM-dd", out endDateOnly);

            if (!success) throw new Exception("Incorrect end date format yyyy-MM-dd");

            if (startDateOnly > endDateOnly) throw new Exception("Start date should not be later than end date");

            DateTime startDate = startDateOnly.ToDateTime(TimeOnly.MinValue);
            DateTime endDate = endDateOnly.ToDateTime(TimeOnly.MaxValue);

            var tasks = await _dbContext.Tasks
                .Where(t => t.Deadline >= startDate && t.CreatedDate <= endDate)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();

        }

        public async Task<IEnumerable<TaskViewDto>> SearchByGroupName(string? groupName, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(groupName)) throw new ArgumentNullException("Search query null");

            var group = await _dbContext.Groups.Where(g => g.Name.ToLower().Trim() == groupName.ToLower().Trim()).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Group not found");

            if (group.ID == Guid.Empty) throw new KeyNotFoundException($"Group invalid.");


            var tasks = await _dbContext.Tasks
                .Where(t => t.GroupID == group.ID)
                .Include(t => t.Order)
                .Include(t => t.User)
                .Include(t => t.Group)
                .ToListAsync();

            if (!tasks.Any()) return Enumerable.Empty<TaskViewDto>();

            //Transfer to ViewDto to display objects
            var taskViewDtos = GetMapper(tasks);

            var taskPaginatedList = new PaginatedList<TaskViewDto>(taskViewDtos, pageNumber, pageSize);

            return taskPaginatedList.GetPaginatedItems();
        }

        public TaskViewDto GetMapper(Task task)
        {
            var taskViewDto = new TaskViewDto
            {
                ID = task.ID,
                OrderID = task.OrderID,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                CreatorName = task.User?.Name,
                CreatedDate = task.CreatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                Deadline = task.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                GroupName = task.Group?.Name
            };

            return taskViewDto;
        }

        public IEnumerable<TaskViewDto> GetMapper(IEnumerable<Task> tasks)
        {
            var taskViewDtos = tasks.Select(t => new TaskViewDto
            {
                ID = t.ID,
                OrderID = t.OrderID,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                CreatorName = t.User?.Name,
                CreatedDate = t.CreatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                Deadline = t.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                GroupName = t.Group?.Name

            });

            return taskViewDtos;
        }

    }




}
