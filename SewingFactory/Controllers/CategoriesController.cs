using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models.DTOs;
using SewingFactory.Models.Models;
using SewingFactory.Repositories.DBContext;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO categoryDTO)
        {
            // Validate that Name is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }
            var categoryNameExists = await _context.Categories.AnyAsync(c => c.Name.Equals(categoryDTO.Name));
            if (categoryNameExists)
            {
                return BadRequest(new { message = "This category name is already exists." });
            }

            Category newCategory = new Category(categoryDTO.Name);
            _context.Entry(newCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
        {
            // Validate that Name is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(categoryDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }
            var categoryNameExists = await _context.Categories.AnyAsync(c => c.Name.Equals(categoryDTO.Name));
            if (categoryNameExists)
            {
                return BadRequest(new { message = "This category name is already exists." });
            }

            Category newCategory = new Category(categoryDTO.Name);
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = newCategory.ID }, newCategory);
        }

        //DELETE: api/Categories/5 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id) //hard-delete
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
