using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.Models
{
    public class Product
    {
        [Key]
        public Guid ID { get; set; } // Primary key
        public string? Name { get; set; }
        public Guid CategoryID { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; } // Navigation property, one product can be in many orders
        public virtual Category Category { get; set; } // Navigation property, one product can have one category

        public Product() { }

        public Product(string? name, Guid categoryID, double? price)
        {
            ID = Guid.NewGuid();
            Name = name;
            CategoryID = categoryID;
            Price = price;
            Status = true; // Default status is true
        }
    }
}
