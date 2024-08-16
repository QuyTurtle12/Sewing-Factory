
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SewingFactory.Models
{
    public class Task
    {
        [Key]
        public Guid ID { get; set; }
        public Guid orderID { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public double? status { get; set; }
        public Guid creatorID { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? deadline { get; set; }
        public Guid groupID { get; set; }
        public virtual Order order { get; set; } // Navigation property, one task is based on one order
        public virtual User user { get; set; } // Navigation property, one task is created by one user
        public virtual Group group { get; set; } // Navigation property, one task is assigned for one group

        public Task() { }

        public Task(Guid id, Guid orderID, string? name, string? description, DateTime? createdDate, DateTime? deadline, Guid creatorID, Guid groupID, double? status)
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

    }
}
