using CoffeeShop.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [EnableCors("AllowSpecificOrigin")]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] Filter filter, [FromQuery]int pageNumber, [FromQuery]int pageSize)
        {
            try
            {
                var products = await productService.GetProductsAsync(filter, pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
