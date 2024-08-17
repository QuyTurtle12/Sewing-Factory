
namespace SewingFactory.Services.Dto.UserDto.RespondDto
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public double? Salary { get; set; }
        public string? RoleName { get; set; } // Add role name
        public string? GroupName { get; set; } // Add group name
    }

}
