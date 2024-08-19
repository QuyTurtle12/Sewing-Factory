using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models.Models
{
    public class Group
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public string? Name { get; set; }

        public Group() { }

        public Group(Guid id, string? Name)
        {
            ID = id;
            this.Name = Name;
        }

    }
}
