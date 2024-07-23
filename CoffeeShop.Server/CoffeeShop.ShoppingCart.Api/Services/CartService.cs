using CoffeeShop.ShoppingCart.Api.Models;
using CoffeeShop.ShoppingCart.Api.Repository;

namespace CoffeeShop.ShoppingCart.Api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartCosmosDbRepository cartCosmosDbRepository;
        //private readonly string? cacheApiUrl;

        public CartService(ICartCosmosDbRepository cartCosmosDbRepository)
        {
            this.cartCosmosDbRepository = cartCosmosDbRepository;
            //  this.cacheApiUrl = configuration.GetValue<string>("CacheApiUrl");
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            var cacheKey = $"cart:{userId}";

            //var cart = await cacheService.GetCachedDataAsync<Cart>(cacheKey);
            //if (cart != null)
            //{
            //   return cart;
            //}

            var cartFromDb = await cartCosmosDbRepository.GetCartAsync(userId);
            if (cartFromDb != null)
            {
                //  await cacheService.SetCachedDataAsync(cacheKey, cartFromDb);
            }

            return cartFromDb;
        }

        public async Task AddOrUpdateCartAsync(Cart cart)
        {
            await cartCosmosDbRepository.AddOrUpdateCartAsync(cart);
            var cacheKey = $"cart:{cart.UserId}";
            //await cacheService.SetCachedDataAsync(cacheKey, cart);
        }

        public async Task RemoveCartAsync(int userId)
        {
            await cartCosmosDbRepository.RemoveCartAsync(userId);
            //await cacheService.InvalidateCacheAsync($"cart:{userId}");
        }
    }
}
