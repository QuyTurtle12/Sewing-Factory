using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Services.Dto.UserDto
{
    public class CreateDto
    {
        public required string Name { get; set; }

        public required Guid RoleID { get; set; }

        public Guid? GroupID { get; set; }

        [StringLength(50, MinimumLength = 5)]
        public required string Username { get; set; }

        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        [Range(0, double.MaxValue)]
        public double? Salary { get; set; }
    }
}
