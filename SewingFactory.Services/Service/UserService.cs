using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto.UserDto;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SewingFactory.Services.Service
{
    public class UserService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public UserService(DatabaseContext dbContext, IConfiguration configuration, ITokenService tokenService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public List<User> GetAllUsers() => _dbContext.Users.ToList();

        public User GetUserById(Guid id)
        {
            var user = _dbContext.Users.Find(id) ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");
            return user;
        }

        public User UpdateUser(Guid id, UpdateDto request)
        {
            var user = _dbContext.Users.Find(id) ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");

            // Validate the request DTO
            ValidateUpdateUserDto(id, request);

            // Update user properties only if they are provided
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
            _dbContext.SaveChanges();

            return user;
        }

        public User CreateUser(CreateDto createUser)
        {
            if (createUser == null) throw new ArgumentNullException(nameof(createUser));

            // Validate the request DTO
            ValidateCreateUserDto(createUser);

            var user = new User
            {
                Name = createUser.Name,
                RoleID = createUser.RoleID,
                GroupID = createUser.GroupID ?? Guid.Empty,
                Username = createUser.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(createUser.Password),
                Salary = createUser.Salary
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public void DeleteUser(Guid id)
        {
            var user = _dbContext.Users.Find(id) ?? throw new KeyNotFoundException($"User with ID '{id}' not found.");
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
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


        public string Login(LoginDto loginDto)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _tokenService.GenerateJwtToken(user);
        }
    }

    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateJwtToken(User user)
        {
            var secret = _configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
