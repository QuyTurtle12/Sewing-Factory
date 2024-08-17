using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class User
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public string? Name { get; set; }
        public Guid RoleID { get; set; }
        public Guid GroupID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double? Salary { get; set; }

        public User() { }

        public User(Guid id, string name, Guid roleID, Guid groupID, string username, string password, double? salary)
        {
            ID = id;
            Name = name;
            RoleID = roleID;
            GroupID = groupID;
            Username = username;
            Password = password;
            Salary = salary;
        }
    }
}
