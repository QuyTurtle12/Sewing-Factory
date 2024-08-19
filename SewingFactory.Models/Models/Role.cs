using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models.Models
{
    public class Role
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public string? Name { get; set; }

        public Role() { }

        public Role(Guid id, string? name)
        {
            ID = id;
            Name = name;
        }
    }
}
