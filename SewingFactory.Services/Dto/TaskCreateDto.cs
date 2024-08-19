using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Services.Dto
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
