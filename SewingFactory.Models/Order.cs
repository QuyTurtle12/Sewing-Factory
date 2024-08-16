

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
        public double? totalPrice;

        public Order() { }

        public Order(Guid id, DateTime? orderDate, DateTime? finishedDate, int userID, string? status, double? totalPrice)
        {
            ID = id;
            this.orderDate = orderDate;
            this.finishedDate = finishedDate;
            this.userID = userID;
            this.status = status;
            this.totalPrice = totalPrice;
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
            get { return totalPrice; }
            set { totalPrice = value; }
        }

    }
}
