using CoffeeShop.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await productService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Filter filter)
        {
            try
            {
                var products = await productService.GetProductsAsync(filter);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
