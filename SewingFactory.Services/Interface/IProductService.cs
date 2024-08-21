using SewingFactory.Models;
using SewingFactory.Models.DTOs;

namespace SewingFactory.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<object>> GetProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<object>> GetAllExistProductsAsync(int pageNumber, int pageSize);
        Task<ProductViewDto> GetProductAsync(Guid id);
        Task<bool> UpdateProductAsync(Guid id, ProductDto productDTO);
        Task<Product> CreateProductAsync(ProductDto productDTO);
        Task<bool> ChangeProductStatusAsync(Guid id);
        Task<bool> ProductExistsAsync(Guid id);
        Task<bool> CategoryExistsAsync(Guid id);
        //Task<Product?> GetProduct(Guid productID);
        Task<String> GetProductName(Guid id);
        Task<IEnumerable<ProductViewDto>> SearchProduct(
                int pageNumber,
                int pageSize,
                string searchByName,
                double lowestPrice,
                double highestPrice,
                string searchByCategoryName);
    }


}