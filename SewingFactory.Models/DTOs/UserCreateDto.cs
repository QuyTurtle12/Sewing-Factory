using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new user.
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// <para>This field is required for creating a user.</para>
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the role ID associated with the user.
        /// <para>This field is required and represents the role assigned to the user.</para>
        /// </summary>
        public required Guid RoleID { get; set; }

        /// <summary>
        /// Gets or sets the group ID associated with the user.
        /// <para>This field is required and represents the group assigned to the user.</para>
        /// </summary>
        public required Guid GroupID { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// <para>This field is required and must be between 5 and 50 characters long.</para>
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// <para>This field is required and must be between 6 and 100 characters long.</para>
        /// </summary>
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the salary of the user.
        /// <para>This field is optional and must be a non-negative value.</para>
        /// </summary>
        [Range(0, double.MaxValue)]
        public double? Salary { get; set; }

        /// <summary>
        /// Gets or sets the status of the user.
        /// <para>This field is required and indicates if the user is active or not.</para>
        /// </summary>
        public bool? Status { get; set; }
    }
}
