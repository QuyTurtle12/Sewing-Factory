using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models.Models
{
    public class Group
    {
        [Key]
        public Guid ID { get; set; } // Primary key
        public string? Name { get; set; }

        // Navigation property: one group can be associated with many users
        public virtual ICollection<User> Users { get; set; }

        public Group()
        {
            Users = new HashSet<User>();
        }

        public Group(Guid id, string? name)
        {
            ID = id;
            Name = name;
            Users = new HashSet<User>();
        }
    }
}
