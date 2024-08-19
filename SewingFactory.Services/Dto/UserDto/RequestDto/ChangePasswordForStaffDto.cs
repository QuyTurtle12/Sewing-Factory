using System.ComponentModel.DataAnnotations;


namespace SewingFactory.Services.Dto.UserDto.RequestDto
{
    public class ChangePasswordForStaffDto
    {
        public required string OldPassword { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public required string NewPassword { get; set; }
    }
}
