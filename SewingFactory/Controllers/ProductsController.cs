using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models;
using SewingFactory.Models.DTOs;
using SewingFactory.Services.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [Authorize(Policy = "Product")]
        // GET: api/Products
        [HttpGet]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Get all products in pages, pageSize = -1 to get all products."
        )]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize == -1)
            {
                pageNumber = 0;
            }
            else if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be greater than 0.");
            }

            var productsWithCategory = await _productService.GetProductsAsync(pageNumber, pageSize);
            return Ok(productsWithCategory);
        }

        [HttpGet("GetAllExistProducts")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get all products with status = true in pages."
        )]
        public async Task<ActionResult<IEnumerable<object>>> GetAllExistProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be greater than 0.");
            }

            var productsWithCategory = await _productService.GetAllExistProductsAsync(pageNumber, pageSize);
            return Ok(productsWithCategory);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Authorization: Anyone",
            Description = "Get product by ID."
        )]
        public async Task<ActionResult<ProductDetailsDTO>> GetProduct(Guid id)
        {
            try
            {
                var productWithCategory = await _productService.GetProductAsync(id);
                return Ok(productWithCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "Product")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Update an existing product by ID."
        )]
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, ProductDTO productDTO)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(id, productDTO);
                if (result)
                {
                    var updatedProduct = await _productService.GetProductAsync(id);
                    return Ok(updatedProduct);
                }
                return NotFound(new { message = "Product not found." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "Product")]
        // POST: api/Products
        [HttpPost]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Create a new product."
        )]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            try
            {
                var newProduct = await _productService.CreateProductAsync(productDTO);
                var category = await _categoryService.GetCategoryByIdAsync(newProduct.CategoryID);
                return Ok(new { newProduct.ID, newProduct.Name, categoryName = category.Name, newProduct.Price, newProduct.Status });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "Product")]
        // PUT: api/Products/ChangeStatus/5
        [HttpPut("ChangeStatus/{id}")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Change the status of a product by ID."
        )]
        public async Task<IActionResult> ChangeProductStatus(Guid id)
        {
            try
            {
                var result = await _productService.ChangeProductStatusAsync(id);
                if (result)
                {
                    var updatedProduct = await _productService.GetProductAsync(id);
                    return Ok(updatedProduct);
                }
                return NotFound(new { message = "Product not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Policy = "Product")]
        [HttpGet("searchProduct")]
        [SwaggerOperation(
            Summary = "Authorization: Product Manager",
            Description = "Search products with filters and pagination."
        )]
        public async Task<ActionResult<IEnumerable<ProductDetailsDTO>>> SearchProduct(
            int pageNumber = 1,
            int pageSize = 10,
            string searchByName = null,
            double lowestPrice = -1,
            double highestPrice = -1,
            string searchByCategoryName = null)
        {
            if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be greater than 0.");
            }

            var products = await _productService.SearchProduct(pageNumber, pageSize, searchByName, lowestPrice, highestPrice, searchByCategoryName);
            if (products == null || !products.Any())
            {
                return NoContent();
            }
            return Ok(products);
        }
    }
}