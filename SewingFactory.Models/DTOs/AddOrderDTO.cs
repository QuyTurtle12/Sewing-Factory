namespace SewingFactory.Models.DTOs
{
    public class AddOrderDTO
    {
        public Guid ProductID { get; set; }
        public int? Quantity { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
    }
}