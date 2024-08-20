using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models.DTOs;
using SewingFactory.Services.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get all categories with pagination; set pageSize to -1 to get all"
        )]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize != -1 && (pageNumber < 1 || pageNumber > pageSize))
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }

            var categories = await _categoryService.GetCategoriesAsync(pageNumber, pageSize);
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get category by ID"
        )]
        public async Task<ActionResult<object>> GetCategory(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(new { category.ID, category.Name });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "Product")]
        // PUT: api/Categories/5
        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Update category"
        )]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO categoryDTO)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryDTO);
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(new { category.ID, category.Name });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "Product")]
        // POST: api/Categories
        [HttpPost]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Create a new category"
        )]
        public async Task<ActionResult<object>> PostCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var newCategory = await _categoryService.CreateCategoryAsync(categoryDTO);
                return Ok(new { newCategory.ID, newCategory.Name });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "Product")]
        [HttpGet("searchCategory")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Search for categories by name with pagination"
        )]
        public async Task<ActionResult<IEnumerable<object>>> SearchCategory(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            if (pageNumber < 1 || pageNumber > pageSize)
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }

            var categories = await _categoryService.SearchCategory(pageNumber, pageSize, searchTerm);
            if (categories == null || !categories.Any())
            {
                return NoContent();
            }

            return Ok(categories);
        }

        [Authorize(Policy = "Product")]
        // DELETE: api/Categories/5
        [HttpDelete("{id:guid}")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Delete a category by ID"
        )]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
