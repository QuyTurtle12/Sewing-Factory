using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.Models
{
    public class Order
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public DateTime? OrderDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Guid ProductID { get; set; }
        public int? Quantity { get; set; }
        public double? TotalAmount { get; set; }
        public Guid UserID { get; set; } // Person who created this order
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public User User { get; set; } // Navigation property, one order is created by one user

        public Order() { }

        public Order(Guid id, DateTime? orderDate, DateTime? finishedDate, Guid userID, string? status, double? totalAmount, Guid productID, int quantity, string? customerName, string? customerPhone)
        {
            ID = id;
            OrderDate = orderDate;
            FinishedDate = finishedDate;
            UserID = userID;
            Status = status;
            TotalAmount = totalAmount;
            ProductID = productID;
            Quantity = quantity;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
        }
    }
}
