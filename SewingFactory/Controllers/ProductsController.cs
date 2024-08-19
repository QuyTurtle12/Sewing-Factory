using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models.DTO;
using SewingFactory.Models.Models;
using SewingFactory.Services;
using SewingFactory.Services.Interface;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Policy = "Product")]
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var productsWithCategory = await _productService.GetProductsAsync(pageNumber, pageSize);
            return Ok(productsWithCategory);
        }

        [HttpGet("GetAllExistProducts")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllExistProducts(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var productsWithCategory = await _productService.GetAllExistProductsAsync(pageNumber, pageSize);
            return Ok(productsWithCategory);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduct(Guid id)
        {
            var productWithCategory = await _productService.GetProductAsync(id);

            if (productWithCategory == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(productWithCategory);
        }

        [Authorize(Policy = "Product")]
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }

            if (productDTO.CategoryID == Guid.Empty)
            {
                return BadRequest(new { message = "CategoryID is required and cannot be an empty GUID." });
            }

            if (productDTO.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            var categoryExists = await _productService.CategoryExistsAsync(productDTO.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }

            var result = await _productService.UpdateProductAsync(id, productDTO);
            if (!result)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok();
        }

        [Authorize(Policy = "Product")]
        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            if (string.IsNullOrWhiteSpace(productDTO.Name))
            {
                return BadRequest(new { message = "Name is required and cannot be empty or whitespace." });
            }

            if (productDTO.CategoryID == Guid.Empty)
            {
                return BadRequest(new { message = "CategoryID is required and cannot be an empty GUID." });
            }

            if (productDTO.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than 0." });
            }

            var categoryExists = await _productService.CategoryExistsAsync(productDTO.CategoryID);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }

            var newProduct = await _productService.CreateProductAsync(productDTO);
            return Ok(newProduct);
        }

        [Authorize(Policy = "Product")]
        // PUT: api/Products/ChangeStatus/5
        [HttpPut("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeProductStatus(Guid id)
        {
            var result = await _productService.ChangeProductStatusAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Product not found." });
            }

            return NoContent();
        }

        [Authorize(Policy = "Product")]
        [HttpGet("searchProduct")]
        public async Task<ActionResult<IEnumerable<ProductDetailsDTO>>> SearchProduct(int pageNumber = 1, int pageSize = 10, string searchByName = null, double lowestPrice = -1, double highestPrice = -1, string searchByCategoryName = null)
        {
            if (pageNumber < 1 || pageNumber > pageSize) //Validate page number
            {
                return BadRequest("pageNumber must be between 1 and " + pageSize);
            }
            var products = await _productService.SearchProduct(pageNumber, pageSize, searchByName, lowestPrice, highestPrice, searchByCategoryName);
            if (products == null)
            {
                return NoContent();
            }
            return Ok(products);
        }
    }
}
