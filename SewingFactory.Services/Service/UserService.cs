using Azure.Core;
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



        public UserService(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration;
        }

        public User CreateUser(CreateUserDto createUser)
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

        private void ValidateCreateUserDto(CreateUserDto request)
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

        public string Login(LoginDto loginDto)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
