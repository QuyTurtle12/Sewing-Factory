using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Models.DTOs
{
    public class TaskCreateDto
    {

        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid GroupID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is required and cannot be empty.")]
        public string? Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Description is required and cannot be empty.")]
        public string? Description { get; set; }

        //Use createDto to separate with other dtos, since creating procedure will required a different set of params to updating procedure
    }
}