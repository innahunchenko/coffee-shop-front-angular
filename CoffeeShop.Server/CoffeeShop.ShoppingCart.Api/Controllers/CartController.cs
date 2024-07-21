using CoffeeShop.ShoppingCart.Api.Models;
using CoffeeShop.ShoppingCart.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Google.Protobuf.WellKnownTypes;
using static ProductsClient.ProductService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoffeeShop.ShoppingCart.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly ProductServiceClient productsClient;

        public CartController(/*ICartService cartService, */ProductServiceClient productServiceClient)
        {
            //this.cartService = cartService;
            this.productsClient = productServiceClient;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var emptyRequest = new Empty();
                Console.WriteLine("Hello from GetCart");
                var categories = await productsClient.GetCategoriesAsync(emptyRequest);


                //var cart = await cartService.GetCartAsync(userId);
                //if (cart == null)
                //{
                //    return NotFound();
                //}

                return Ok(categories);
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"gRPC ошибка: {ex.Status.Detail}");
                Console.WriteLine($"Код статуса: {ex.StatusCode}");
                return BadRequest(ex.Status.Detail);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCart([FromBody] Cart cart)
        {
            await cartService.AddOrUpdateCartAsync(cart);
            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveCart(int userId)
        {
            await cartService.RemoveCartAsync(userId);
            return NoContent();
        }
    }
}
