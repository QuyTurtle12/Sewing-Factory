

namespace SewingFactory.Models.DTO
{
    public class AddOrderDTO
    {
        public Guid ProductID { get; set; }
        public int? Quantity { get; set; }
        public Guid UserID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
    }
}
