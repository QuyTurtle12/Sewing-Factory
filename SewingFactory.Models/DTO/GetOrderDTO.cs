using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Models.DTO
{
    public class GetOrderDTO
    {
        public DateTime? OrderDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public double? TotalAmount { get; set; }
        public string? CreatorName { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
    }
}