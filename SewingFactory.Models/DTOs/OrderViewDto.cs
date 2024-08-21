namespace SewingFactory.Models.DTOs
{
    public class OrderViewDto
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