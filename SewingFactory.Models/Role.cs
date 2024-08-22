using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Role
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public string? Name { get; set; }
        public virtual ICollection<User> Users { get; set; } // Navigation property, one role has many users

        public Role() { }

        public Role(Guid id, string? name)
        {
            ID = id;
            Name = name;
        }
    }
}
