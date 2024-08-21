namespace SewingFactory.Models.DTOs
{
    public class TaskViewDto
    {
        public Guid ID { get; set; }
        public Guid? OrderID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Status { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatedDate { get; set; }
        public string? Deadline { get; set; }
        public string? GroupName { get; set; }

    }
}