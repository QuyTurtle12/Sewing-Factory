

using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Order
    {
        [Key]
        public Guid ID { get; set; } //primary key
        public DateTime? orderDate { get; set; }
        public DateTime? finishedDate { get; set; }
        public Guid productID { get; set; }
        public int? quantity { get; set; }
        public double? totalAmount { get; set; }
        public Guid userID { get; set; }
        public string? status { get; set; }
        public virtual ICollection<Task> tasks { get; set; } // Navigation property, one order has many tasks
        public virtual Product product { get; set; } // Navigation property, one order has one product
        public User user { get; set; } // Navigation property, one order is created by one user

        public Order() { }

        public Order(Guid id, DateTime? orderDate, DateTime? finishedDate, Guid userID, string? status, double? totalAmount)
        {
            ID = id;
            this.orderDate = orderDate;
            this.finishedDate = finishedDate;
            this.userID = userID;
            this.status = status;
            this.totalAmount = totalAmount;
        }

    }
}
