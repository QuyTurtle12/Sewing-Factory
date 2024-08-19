using SewingFactory.Models.Models;
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
        public bool? Status { get; set; }

        public virtual Role Role { get; set; } // Navigation property, one user has one role
        public virtual Group Group { get; set; } // Navigation property, one user is associated with one group
        public virtual ICollection<Task> Tasks { get; set; } // Navigation property, one user can create many tasks
        public virtual ICollection<Order> Orders { get; set; } // Navigation property, one user can create many orders

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