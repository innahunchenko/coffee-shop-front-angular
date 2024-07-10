using CoffeeShop.ShoppingCart.Api.Models;

namespace CoffeeShop.ShoppingCart.Api.Repository
{
    public interface ICartRedisCacheRepository
    {
        Task AddOrUpdateCartAsync(int userId, Cart cart);
        Task<Cart> GetCartAsync(int userId);
        Task RemoveCartAsync(int userId);
    }
}