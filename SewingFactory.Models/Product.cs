

using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Product
    {
        [Key]
        public Guid ID; //primary key
        public string? name;
        public Guid categoryID;
        public double? price;
        public virtual ICollection<OrderDetail> detail { get; set; } // Navigation property, one product can be in many order detail
        public virtual Category category { get; set; } // Navigation property, one product can have one category
       
        public Product() { }

        public Product(Guid id, string? name, Guid categoryID, double? price)
        {
            ID = id;
            this.name = name;
            this.categoryID = categoryID;
            this.price = price;
        }


        public string? Name
        {
            get { return name; }
            set { name = value; }
        }

        public double? Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
