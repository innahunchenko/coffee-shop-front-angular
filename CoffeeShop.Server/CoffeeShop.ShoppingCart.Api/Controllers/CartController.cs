using CoffeeShop.ShoppingCart.Api.Models;
using CoffeeShop.ShoppingCart.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.ShoppingCart.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await cartService.GetCartAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
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
