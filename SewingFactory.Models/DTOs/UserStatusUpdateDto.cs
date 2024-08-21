using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating the status of a user.
    /// </summary>
    public class UserStatusUpdateDto
    {
        /// <summary>
        /// Gets or sets the status of the user.
        /// <para>Required field, representing whether the user is active (true) or inactive (false).</para>
        /// </summary>
        [Required]
        public bool Status { get; set; }
    }
}
