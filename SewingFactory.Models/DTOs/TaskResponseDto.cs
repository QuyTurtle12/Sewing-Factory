namespace SewingFactory.Models.DTOs
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