

namespace SewingFactory.Models.DTO
{
    public class UpdateOrderDTO
    {
        public Guid ID { get; set; } 

        public Guid UserID { get; set; }

        public string? Status { get; set; }
    }
}
