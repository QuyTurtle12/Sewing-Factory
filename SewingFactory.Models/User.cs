using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class User
    {
        [Key]
        public Guid ID; //primary key
        public string? name;
        public Guid roleID;
        public int? groupID;
        public string username;
        public string password;
        public double? salary;
        public Role role { get; set; } // Navigation property, one user has one role
        public Group group { get; set; } // Navigation property, one user is associated with one group
        public ICollection<Task> tasks { get; set; } // Navigation property, one user can creates many tasks
        public Order order { get; set; } // Navigation property, one user is associated with one order


        public User() { }
        public User(Guid id, string name, Guid roleID, int groupID, string username, string password, double? salary)
        {
            ID = id;
            this.name = name;
            this.roleID = roleID;
            this.groupID = groupID;
            this.username = username;
            this.password = password;
            this.salary = salary;
        }

        public string? Name
        {
            get { return name; }
            set { name = value; }
        }

        public int? GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public double? Salary
        {
            get { return salary; }
            set { salary = value; }
        }
    }

}
