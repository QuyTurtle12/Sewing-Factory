using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto.UserDto.RequestDto;
using SewingFactory.Services.Dto.UserDto.RespondDto;
using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Services.Service
{
    public class UserService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;

        // Constructor for dependency injection
        public UserService(DatabaseContext dbContext, ValidationService validation, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _validationService = validation ?? throw new ArgumentNullException(nameof(validation));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves all users from the database including their roles and groups.
        /// </summary>
        /// <returns>A list of UserDto objects.</returns>
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        /// <summary>
        /// Retrieves a paginated list of users from the database including their roles and groups.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A paginated list of UserDto objects.</returns>
        public async Task<(List<UserDto> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _dbContext.Users.CountAsync();

            var users = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (_mapper.Map<List<UserDto>>(users), totalCount);
        }

        /// <summary>
        /// Retrieves a user by their ID including their role and group.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A UserDto object representing the user.</returns>
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Updates a user's details based on the provided ID and update request.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The update details.</param>
        /// <returns>The updated UserDto object.</returns>
        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateDto request)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            // Validate the request DTO
            _validationService.ValidateUpdateUserDto(id, request);

            // Map properties from request DTO to user entity
            // Only update fields that are provided in the request
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                user.Name = request.Name;
            }

            if (request.RoleID.HasValue)
            {
                user.RoleID = request.RoleID.Value;
            }

            if (request.GroupID.HasValue)
            {
                user.GroupID = request.GroupID.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                user.Username = request.Username;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                _validationService.ValidatePassword(request.Password);
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            if (request.Salary.HasValue)
            {
                user.Salary = request.Salary.Value;
            }

            // Save changes to the database
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Creates a new user based on the provided creation details.
        /// </summary>
        /// <param name="createUser">The user creation details.</param>
        /// <returns>The created UserDto object.</returns>
        public async Task<UserDto> CreateUserAsync(CreateDto createUser)
        {
            if (createUser == null) throw new ArgumentNullException(nameof(createUser));

            // Validate the request DTO
            _validationService.ValidateCreateUserDto(createUser);

            var user = _mapper.Map<User>(createUser);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Updates the status of a user based on the provided ID and status details.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="statusDto">The status update details.</param>
        /// <returns>The updated UserDto object.</returns>
        public async Task<UserDto> UpdateUserStatusAsync(Guid id, UpdateUserStatusDto statusDto)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            user.Status = statusDto.Status;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            // Create a UserDto and set the role and group names manually
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public class ChangePasswordResult
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
        }

        public async Task<ChangePasswordResult> ChangePasswordForStaff(Guid id, ChangePasswordForStaffDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Retrieve the user from the database
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            // Validate the old password
            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
            {
                return new ChangePasswordResult
                {
                    Success = false,
                    Message = "Old password is incorrect."
                };
            }

            // Validate the new password (additional checks if needed)
            try
            {
                _validationService.ValidatePassword(request.NewPassword);
            }
            catch (ValidationException ex)
            {
                return new ChangePasswordResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            // Update the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Save changes to the database
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            // Return success result
            return new ChangePasswordResult
            {
                Success = true,
                Message = "Password changed successfully."
            };
        }



        /// <summary>
        /// Deletes a user based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        public async System.Threading.Tasks.Task DeleteUserAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();


        }

        // Accquire user name
        public async Task<string?> GetUserName(Guid userID)
        {
            var user = await GetUserByIdAsync(userID);
            return user.Username;
        }

        // Validate if user is existed in database
        public async Task<bool> IsValidUser(Guid userID)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == userID) is not null;
        }
    }
}