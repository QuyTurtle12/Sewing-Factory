using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Category
    {
        [Key]
        public Guid ID { get; set; } //primary key

        public string? name { get; set; }
        public virtual ICollection<Product> products {  get; set; } // Navigation property, one category can be in many products
        public Category(){ }

        public Category(Guid id, string? name)
        {
            ID = id;
            this.name = name;
        }
    }
}
