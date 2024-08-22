using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models
{
    public class Task
    {
        [Key]
        public Guid ID { get; set; } // Primary key

        public Guid? OrderID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Status { get; set; }
        public Guid CreatorID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid GroupID { get; set; }

        public virtual Order? Order { get; set; } // Navigation property, one task is based on one order
        public virtual User? User { get; set; } // Navigation property, one task is created by one user
        public virtual Group? Group { get; set; } // Navigation property, one task is assigned to one group

        public Task() { }

        public Task(Guid id, Guid orderID, string? name, string? description, DateTime? createdDate, DateTime? deadline, Guid creatorID, Guid groupID, double? status)
        {
            ID = id;
            OrderID = orderID;
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            Deadline = deadline;
            CreatorID = creatorID;
            GroupID = groupID;
            Status = status;
        }
    }
}
