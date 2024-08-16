using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Category
    {
        [Key]
        public Guid ID { get; set; } //primary key

        public string? Name { get; set; }
        public virtual ICollection<Product> Products {  get; set; } // Navigation property, one category can be in many products
        public Category(){ }

        public Category(Guid id, string? Name)
        {
            ID = id;
            this.Name = Name;
        }
    }
}
