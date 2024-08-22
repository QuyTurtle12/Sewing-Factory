namespace SewingFactory.Models.DTOs
{
    public class TaskUpdateDto
    {
        public Guid? OrderID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Deadline { get; set; }

        //Use updateDto to separate with other dtos, since creating procedure will required a different set of params to updating procedure
        
        
        public bool HasAtLeastOneField()
        {
            return OrderID.HasValue ||
                   !string.IsNullOrWhiteSpace(Name) ||
                   !string.IsNullOrWhiteSpace(Description) ||
                   !string.IsNullOrWhiteSpace(Deadline);
        }

    }
}