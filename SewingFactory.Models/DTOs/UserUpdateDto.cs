using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating user information.
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// <para>Optional field, can be used to update the user's name.</para>
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user's role.
        /// <para>Optional field, can be used to update the user's role.</para>
        /// </summary>
        public Guid? RoleID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user's group.
        /// <para>Optional field, can be used to update the user's group.</para>
        /// </summary>
        public Guid? GroupID { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// <para>Optional field, with a length constraint of 5 to 50 characters.</para>
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// <para>Optional field, with a length constraint of 6 to 20 characters.</para>
        /// </summary>
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the salary of the user.
        /// <para>Optional field, must be a non-negative value.</para>
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        public double? Salary { get; set; }

        /// <summary>
        /// Gets or sets the status of the user.
        /// <para>This field is required and indicates if the user is active or not.</para>
        /// </summary>
        public bool? Status { get; set; }
    }
}
