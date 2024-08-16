using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Category
    {
        [Key]
        public Guid ID; //primary key
        public string? name;
        public virtual ICollection<Product> products {  get; set; } // Navigation property, one category can be in many products
        public Category(){ }

        public Category(Guid id, string? name)
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
