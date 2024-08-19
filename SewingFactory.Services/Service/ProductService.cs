using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.Models;
using SewingFactory.Repositories.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SewingFactory.Models.DTO;


namespace SewingFactory.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetProductsAsync()
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

        public async Task<IEnumerable<object>> GetAllExistProductsAsync()
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
                .ToListAsync();
        }

        public async Task<ProductDetailsDTO> GetProductAsync(Guid id)
        {
            return await _context.Products
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
                return false;
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
                return false;
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
    }
}