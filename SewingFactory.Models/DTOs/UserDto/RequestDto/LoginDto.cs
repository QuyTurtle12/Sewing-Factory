namespace SewingFactory.Services.DTOs.UserDto.RequestDto
{
    /// <summary>
    /// Data Transfer Object for user login information.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// <para>This field is required for authentication.</para>
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// <para>This field is required for authentication.</para>
        /// </summary>
        public required string Password { get; set; }
    }
}
