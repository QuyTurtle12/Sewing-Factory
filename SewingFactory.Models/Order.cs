

using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Order
    {
        [Key]
        public Guid ID; //primary key
        public DateTime? orderDate;
        public DateTime? finishedDate;
        public int userID;
        public string? status;
        public double? totalAmount;
        public virtual ICollection<Task> tasks {  get; set; } // Navigation property, one order has many tasks
        public virtual ICollection<OrderDetail> detail { get; set; } // Navigation property, one order has many detail
        public User user { get; set; } // Navigation property, one order is created by one user
        public Order() { }

        public Order(Guid id, DateTime? orderDate, DateTime? finishedDate, int userID, string? status, double? totalAmount)
        {
            ID = id;
            this.orderDate = orderDate;
            this.finishedDate = finishedDate;
            this.userID = userID;
            this.status = status;
            this.totalAmount = totalAmount;
        }

        public DateTime? OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        public DateTime? FinishedDate
        {
            get { return finishedDate; }
            set { finishedDate = value; }
        }

        public int UserID
        {
            get { return userID; }
        }

        public string? Status
        {
            get { return status; }
            set { status = value; }
        }
        
        public double? TotalPrice
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }

    }
}
