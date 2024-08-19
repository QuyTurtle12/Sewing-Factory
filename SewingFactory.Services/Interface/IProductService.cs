using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.DTO;
using SewingFactory.Models.Models;
using SewingFactory.Repositories.DBContext;

namespace SewingFactory.Services
{
    public interface IProductService
    {
        Task<IEnumerable<object>> GetProductsAsync();
        Task<IEnumerable<object>> GetAllExistProductsAsync();
        Task<ProductDetailsDTO> GetProductAsync(Guid id);
        Task<bool> UpdateProductAsync(Guid id, ProductDTO productDTO);
        Task<Product> CreateProductAsync(ProductDTO productDTO);
        Task<bool> ChangeProductStatusAsync(Guid id);
        Task<bool> ProductExistsAsync(Guid id);
        Task<bool> CategoryExistsAsync(Guid id);
        //Task<Product?> GetProduct(Guid productID);
        Task<String> GetProductName(Guid id);
    }

        
}
