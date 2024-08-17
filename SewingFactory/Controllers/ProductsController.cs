using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            // Check if the product exists
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }
            // Check if the CategoryID exists in the Categories table
            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == product.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }
            if (product.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            // Map the ProductDTO to the existing Product entity
            existingProduct.Name = product.Name;
            existingProduct.CategoryID = product.CategoryID;
            existingProduct.Price = product.Price;

            _context.Entry(existingProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound(new { message = "Product not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Check if the CategoryID exists in the Categories table
            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == product.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }
            if(product.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            Product newProduct = new Product(product.Name, product.CategoryID, product.Price);

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = newProduct.ID }, newProduct);
        }

        // DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(Guid id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
