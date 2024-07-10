using CoffeeShop.ShoppingCart.Api.Models;

namespace CoffeeShop.ShoppingCart.Api.Repository
{
    public interface ICartCosmosDbRepository
    {
        Task AddOrUpdateCartAsync(Cart cart);
        Task<Cart> GetCartAsync(int userId);
        Task RemoveCartAsync(int userId);
    }
}