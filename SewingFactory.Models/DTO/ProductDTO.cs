using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Models.DTO
{
    public class ProductDTO
    {
        public string? Name { get; set; }
        public Guid CategoryID { get; set; }
        public double? Price { get; set; }
    }
}
