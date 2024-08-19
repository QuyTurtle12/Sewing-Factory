
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.DTOs;
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
        public async Task<ActionResult<IEnumerable<object>>> GetProducts()
        {
            var productsWithCategory = await _context.Products
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


            return Ok(productsWithCategory);
        }

        [HttpGet("GetAllExistProducts")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllExistProducts()
        {
            var productsWithCategory = await _context.Products
                .Where(p => p.Status == true) // Filter products with status == true
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


            return Ok(productsWithCategory);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduct(Guid id)
        {
            var productWithCategory = await _context.Products
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
                .FirstOrDefaultAsync(p => p.ID == id);

            if (productWithCategory == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(productWithCategory);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, ProductDTO productDTO)
        {
            // Validate that Name is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }

            // Validate that CategoryID is not an empty GUID
            if (productDTO.CategoryID == Guid.Empty)
            {
                return BadRequest(new { message = "CategoryID is required and cannot be an empty GUID." });
            }

            // Validate that Price is greater than 0
            if (productDTO.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == productDTO.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }

            existingProduct.Name = productDTO.Name;
            existingProduct.CategoryID = productDTO.CategoryID;
            existingProduct.Price = productDTO.Price;

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
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            // Validate that Name is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }

            // Validate that CategoryID is not an empty GUID
            if (productDTO.CategoryID == Guid.Empty)
            {
                return BadRequest(new { message = "CategoryID is required and cannot be an empty GUID." });
            }

            // Validate that Price is greater than 0
            if (productDTO.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            // Check if the CategoryID exists in the Categories table
            var categoryExists = await _context.Categories.AnyAsync(c => c.ID == productDTO.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }

            Product newProduct = new Product(productDTO.Name, productDTO.CategoryID, productDTO.Price);

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = newProduct.ID }, newProduct);
        }

        // PUT: api/Products/ChangeStatus/5
        [HttpPut("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeProductStatus(Guid id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            existingProduct.Status = !existingProduct.Status;

            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
