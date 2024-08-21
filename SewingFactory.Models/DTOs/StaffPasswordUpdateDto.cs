using System.ComponentModel.DataAnnotations;


namespace SewingFactory.Models.DTOs
{
    public class StaffPasswordUpdateDto
    {
        public required string OldPassword { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public required string NewPassword { get; set; }
    }
}
