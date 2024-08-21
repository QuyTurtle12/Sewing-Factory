namespace SewingFactory.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for representing a user.
    /// </summary>
    public class UserViewDto
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public double? Salary { get; set; }
        public bool? Status { get; set; }
        public RoleDto? Role { get; set; }
        public GroupDto? Group { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for representing a role.
    /// </summary>
    public class RoleDto
    {
        public Guid RoleID { get; set; }
        public string? RoleName { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for representing a group.
    /// </summary>
    public class GroupDto
    {
        public Guid GroupID { get; set; }
        public string? GroupName { get; set; }
    }

}
