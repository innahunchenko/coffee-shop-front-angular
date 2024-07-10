using CoffeeShop.ShoppingCart.Api.Models;

namespace CoffeeShop.ShoppingCart.Api.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(int userId);
        Task AddOrUpdateCartAsync(Cart cart);
        Task RemoveCartAsync(int userId);
    }
}
