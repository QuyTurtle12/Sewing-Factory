using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models.Models
{
    public class Role
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public string? Name { get; set; }

        // Navigation property: one role can be associated with many users
        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
        }

        public Role(Guid id, string? name)
        {
            ID = id;
            Name = name;
            Users = new HashSet<User>();
        }
    }
}
