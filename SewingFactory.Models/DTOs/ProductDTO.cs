namespace SewingFactory.Models.DTOs
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public Guid CategoryID { get; set; }
        public double? Price { get; set; }
    }
}