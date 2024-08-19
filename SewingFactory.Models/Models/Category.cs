using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.Models
{
    public class Category
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public string? Name { get; set; }
        public Category() { }
        public Category(string? Name)
        {
            ID = Guid.NewGuid();
            this.Name = Name;
        }
    }
}
