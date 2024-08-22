using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Group
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; } // Navigation property, one group has many users
        public virtual ICollection<Task> Tasks { get; set; } // Navigation property, one group has many tasks

        public Group() { }

        public Group(Guid id, string? Name)
        {
            ID = id;
            this.Name = Name;
        }

    }
}
