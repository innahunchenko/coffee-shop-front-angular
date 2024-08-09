using CoffeeShop.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] string category, [FromQuery]int pageNumber, [FromQuery]int pageSize)
        {
            try
            {
                var products = await productService.GetProductsByCategoryAsync(category, pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subcategory")]
        public async Task<IActionResult> GetProductsBySubcategory([FromQuery] string subcategory, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var products = await productService.GetProductsBySubcategoryAsync(subcategory, pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("productName")]
        public async Task<IActionResult> GetProductsByNameAsync([FromQuery] string productName, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var products = await productService.GetProductsByNameAsync(productName, pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                var products = await productService.GetAllProductsAsync(pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
