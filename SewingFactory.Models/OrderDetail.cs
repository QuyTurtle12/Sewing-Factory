
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class OrderDetail
    {
        //Composition Primary Key
        [Key, Column (Order = 0)]
        public Guid orderID;
        [Key, Column(Order = 1)]
        public Guid productID;
        public int? quantity;
        public double? totalPrice;

        public OrderDetail() { }

        public OrderDetail(Guid orderID, Guid productID, int? quantity, double? totalPrice)
        {
            this.orderID = orderID;
            this.productID = productID;
            this.quantity = quantity;
            this.totalPrice = totalPrice;
        }

        public int? Quantity { 
            get {  return quantity; } 
            set { quantity = value; }
        }

        public double? TotalPrice { 
            get {  return totalPrice; } 
            set {  totalPrice = value; }
        }
    }
}
