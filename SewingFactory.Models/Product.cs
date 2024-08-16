

using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Product
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public string? name { get; set; }
        public Guid categoryID { get; set; }
        public double? price { get; set; }
        public virtual ICollection<Order> orders { get; set; } // Navigation property, one product can be in many orders
        public virtual Category category { get; set; } // Navigation property, one product can have one category
       
        public Product() { }

        public Product(Guid id, string? name, Guid categoryID, double? price)
        {
            ID = id;
            this.name = name;
            this.categoryID = categoryID;
            this.price = price;
        }
    }
}
