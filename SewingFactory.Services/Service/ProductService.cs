using SewingFactory.Repositories.DBContext;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.DTOs;
using SewingFactory.Models;
using SewingFactory.Services.Interface;


namespace SewingFactory.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetProductsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                return await _context.Products
                .Join(
                    _context.Categories,
                    product => product.CategoryID,
                    category => category.ID,
                    (product, category) => new
                    {
                        product.ID,
                        product.Name,
                        CategoryName = category.Name,
                        product.Price,
                        product.Status
                    })
                .ToListAsync();
            }
            else
            {
                return await _context.Products
                .Join(
                    _context.Categories,
                    product => product.CategoryID,
                    category => category.ID,
                    (product, category) => new
                    {
                        product.ID,
                        product.Name,
                        CategoryName = category.Name,
                        product.Price,
                        product.Status
                    })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
        }

        public async Task<IEnumerable<object>> GetAllExistProductsAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .Where(p => p.Status == true)
                .Join(
                    _context.Categories,
                    product => product.CategoryID,
                    category => category.ID,
                    (product, category) => new
                    {
                        product.ID,
                        product.Name,
                        CategoryName = category.Name,
                        product.Price,
                        product.Status
                    })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ProductDetailsDTO> GetProductAsync(Guid id)
        {
            var product = await _context.Products
                .Join(
                    _context.Categories,
                    product => product.CategoryID,
                    category => category.ID,
                    (product, category) => new ProductDetailsDTO
                    {
                        ID = product.ID,
                        Name = product.Name,
                        CategoryName = category.Name,
                        Price = product.Price,
                        Status = product.Status
                    })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
            }

            return product;
        }


        public async Task<String> GetProductName(Guid id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);
            return product.Name;
        }

        public async Task<bool> UpdateProductAsync(Guid id, ProductDTO productDTO)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == productDTO.CategoryID);
            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Category with ID '{productDTO.CategoryID}' not found.");
            }

            if (productDTO.Price <= 0)
            {
                throw new ArgumentException("Product price must greater than 0.");
            }

            existingProduct.Name = productDTO.Name;
            existingProduct.CategoryID = productDTO.CategoryID;
            existingProduct.Price = productDTO.Price;

            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Product> CreateProductAsync(ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                throw new ArgumentException("Product name is required and cannot be empty or whitespace.");
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == productDTO.CategoryID);
            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Category with ID '{productDTO.CategoryID}' not found.");
            }

            if (productDTO.Price <= 0)
            {
                throw new ArgumentException("Product price must greater than 0.");
            }

            Product newProduct = new Product(productDTO.Name, productDTO.CategoryID, productDTO.Price);
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<bool> ChangeProductStatusAsync(Guid id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
            }

            existingProduct.Status = !existingProduct.Status;
            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProductExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(e => e.ID == id);
        }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            return await _context.Categories.AnyAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<ProductDetailsDTO>> SearchProduct(
                int pageNumber,
                int pageSize,
                string searchByName,
                double lowestPrice,
                double highestPrice,
                string searchByCategoryName)
        {
            var query = _context.Products
                .Join(
                    _context.Categories,
                    product => product.CategoryID,
                    category => category.ID,
                    (product, category) => new { product, category.Name }
                )
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchByName))
            {
                query = query.Where(p => p.product.Name.ToUpper().Contains(searchByName.ToUpper()));
            }

            // Search by price range
            if (lowestPrice >= 0)
            {
                query = query.Where(p => p.product.Price >= lowestPrice);
            }
            if (highestPrice >= 0)
            {
                query = query.Where(p => p.product.Price <= highestPrice);
            }

            // Search by category name
            if (!string.IsNullOrEmpty(searchByCategoryName))
            {
                query = query.Where(p => p.Name.ToUpper().Contains(searchByCategoryName.ToUpper()));
            }

            if (!query.Any())
            {
                throw new KeyNotFoundException("No products matched the search criteria.");
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDetailsDTO
                {
                    ID = p.product.ID,
                    Name = p.product.Name,
                    CategoryName = p.Name,
                    Price = p.product.Price,
                    Status = p.product.Status
                })
                .ToListAsync();
        }
    }
}