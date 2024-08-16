using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class Group
    {
        [Key]
        public Guid ID; //primary key
        public string? name;
        public ICollection<User> users { get; set; } // Navigation property, one group has many user

        public Group() { }

        public Group(Guid id, string? name)
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
