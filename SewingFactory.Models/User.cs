using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class User
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public string? name { get; set; }
        public Guid roleID { get; set; }
        public Guid groupID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public double? salary { get; set; }
        public virtual Role role { get; set; } // Navigation property, one user has one role
        public virtual Group group { get; set; } // Navigation property, one user is associated with one group
        public virtual ICollection<Task> tasks { get; set; } // Navigation property, one user can creates many tasks
        public virtual ICollection<Order> orders { get; set; } // Navigation property, one user can create many orders


        public User() { }
        public User(Guid id, string name, Guid roleID, Guid groupID, string username, string password, double? salary)
        {
            ID = id;
            this.name = name;
            this.roleID = roleID;
            this.groupID = groupID;
            this.username = username;
            this.password = password;
            this.salary = salary;
        }

    }

}
