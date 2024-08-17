

using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;


namespace SewingFactory.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _dbContext;

        public ProductService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProduct(Guid productID)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ID == productID);
            return product;
        }

        public async Task<string?> GetProductName(Guid productID)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ID == productID);
            return product.Name;
        }

        public async Task<bool> IsValidProduct(Guid productID)
        {
            var product = await GetProduct(productID);
            if (product is null)
            {
                return false;
            }
            return true;
        }
    }
}
