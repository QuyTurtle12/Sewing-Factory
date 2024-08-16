
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class Role
    {
        [Key]
        public Guid ID;
        public string? name;
        public ICollection<User> users { get; set; } // Navigation property, one role has many user

        public Role() { }

        public Role(Guid id, string? name)
        {
            ID = id;
            this.name = name;
        }

        public string? Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
