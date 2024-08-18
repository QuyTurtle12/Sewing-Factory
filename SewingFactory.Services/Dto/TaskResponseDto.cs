using SewingFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Services.Dto
{
    public class TaskResponseDto
    {
        public Guid ID { get; set; }
        public Guid OrderID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Status { get; set; }
        public string? CreatorName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string? GroupName { get; set; }

    }
}
