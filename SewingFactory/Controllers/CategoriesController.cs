using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models;
using SewingFactory.Models.DTOs;
using SewingFactory.Services.Interface;

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
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(int pageNumber = 1, int pageSize = 10)
        {
            if(pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var categories = await _categoryService.GetCategoriesAsync(pageNumber, pageSize);
            return Ok(categories);
        }
            if (pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var categories = await _categoryService.GetCategoriesAsync(pageNumber, pageSize);
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id:guid}")]
        [ActionName("GetCategoryById")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(new { category.ID, category.Name });
        }

        [Authorize(Policy = "Product")]
        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO categoryDTO)
        {
            Category category;
            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryDTO);
                category = await _categoryService.GetCategoryByIdAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok(new { category.ID, category.Name });
        }

        [Authorize(Policy = "Product")]
        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
        {
            Category newCategory;
            try
            {
                newCategory = await _categoryService.CreateCategoryAsync(categoryDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { newCategory.ID, newCategory.Name });
        }

        [Authorize(Policy = "Product")]
        [HttpGet("searchCategory")]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategory(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            if (pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var categories = await _categoryService.SearchCategory(pageNumber, pageSize, searchTerm);
            if (categories == null)
            {
                return NoContent();
            }
            return Ok(categories);
        }
    }
}