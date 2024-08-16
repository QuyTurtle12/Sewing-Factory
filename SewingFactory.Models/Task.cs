
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class Task
    {
        [Key]
        public Guid ID;
        public string orderID;
        public string? name;
        public string? description;
        public double? status;
        public Guid creatorID;
        public DateTime? createdDate;
        public DateTime? deadline;
        public Guid groupID;
        public virtual Order order { get; set; } // Navigation property, one task is based on one order
        public virtual User user { get; set; } // Navigation property, one task is created by one user
        public virtual Group group { get; set; } // Navigation property, one task is assigned for one group

        public Task() { }

        public Task(Guid id, string orderID, string? name, string? description, DateTime? createdDate, DateTime? deadline, Guid creatorID, Guid groupID, double? status)
        {
            ID = id;
            this.orderID = orderID;
            this.name = name;
            this.description = description;
            this.createdDate = createdDate;
            this.deadline = deadline;
            this.creatorID = creatorID;
            this.groupID = groupID;
            this.status = status;
        }

        public string OrderID { get {  return orderID; } set { orderID = value; } }

        public string? Name { get { return name; } set { name = value; } }

        public string? Description { get { return description; } set { description = value; } }

        public DateTime? CreatedDate { get {  return createdDate; } set {  createdDate = value; } }

        public DateTime? Deadline { get { return deadline; } set {  deadline = value; } }

        public double? Status { get { return status; } set { status = value; } }
    }
}
