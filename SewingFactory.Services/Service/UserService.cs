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
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(DatabaseContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateDto request)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Group)
                .FirstOrDefaultAsync(u => u.ID == id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            // Validate the request DTO
            ValidateUpdateUserDto(id, request);

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

        public async Task<UserDto> CreateUserAsync(CreateDto createUser)
        {
            if (createUser == null) throw new ArgumentNullException(nameof(createUser));

            // Validate the request DTO
            ValidateCreateUserDto(createUser);

            var user = _mapper.Map<User>(createUser);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id)
                ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        private void ValidateCreateUserDto(CreateDto request)
        {
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, context, results, true);
            if (!isValid)
            {
                var errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new ValidationException($"User validation failed: {errorMessages}");
            }

            // Additional custom validations
            if (_dbContext.Users.Any(u => u.Username == request.Username))
            {
                throw new ValidationException($"Username '{request.Username}' is already taken.");
            }
        }

        private void ValidateUpdateUserDto(Guid id, UpdateDto request)
        {
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, context, results, true);
            if (!isValid)
            {
                var errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new ValidationException($"User validation failed: {errorMessages}");
            }

            // Additional custom validations
            if (_dbContext.Users.Any(u => u.Username == request.Username && u.ID != id))
            {
                throw new ValidationException($"Username '{request.Username}' is already taken.");
            }
        }
    }
}
