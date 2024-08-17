

using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using Microsoft.EntityFrameworkCore;


namespace SewingFactory.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _dbContext;

        public ProductService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetProductName(Guid productID)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ID == productID);
            return product.Name;
        }

        public Task<bool> IsValidProduct(Guid productID)
        {
            throw new NotImplementedException();
        }
    }
}
