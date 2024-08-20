using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Services.DTOs.UserDto.RequestDto
{
    /// <summary>
    /// Data Transfer Object for updating the status of a user.
    /// </summary>
    public class UpdateUserStatusDto
    {
        /// <summary>
        /// Gets or sets the status of the user.
        /// <para>Required field, representing whether the user is active (true) or inactive (false).</para>
        /// </summary>
        [Required]
        public bool Status { get; set; }
    }
}
