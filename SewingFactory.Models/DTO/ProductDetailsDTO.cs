using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingFactory.Models.DTO
{
    public class ProductDetailsDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
    }

}
