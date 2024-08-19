using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Dto.UserDto.RequestDto;

namespace SewingFactory.Services.Service
{
    public class ValidationService
    {
        private readonly DatabaseContext _dbContext;

        public ValidationService(DatabaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Validate the creation DTO
        public void ValidateCreateUserDto(CreateDto request)
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

        // Validate the update DTO
        public void ValidateUpdateUserDto(Guid id, UpdateDto request)
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

        // Validate the password
        public void ValidatePassword(string password)
        {
            // Additional password complexity checks can be added here (e.g., requires special characters, digits, etc.)
        }
    }
}
