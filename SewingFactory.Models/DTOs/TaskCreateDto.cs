namespace SewingFactory.Models.DTOs
{
    public class TaskCreateDto
    {
        public Guid OrderID { get; set; }
        public Guid GroupID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        //Use createDto to separate with other dtos, since creating procedure will required a different set of params to updating procedure
    }
}