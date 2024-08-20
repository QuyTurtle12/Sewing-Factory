namespace SewingFactory.Models.DTO
{
    public class ProductDTO
    {
        public string? Name { get; set; }
        public Guid CategoryID { get; set; }
        public double? Price { get; set; }
    }
}