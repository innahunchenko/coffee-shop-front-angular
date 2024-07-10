using CoffeeShop.ShoppingCart.Api.Models;
using CoffeeShop.ShoppingCart.Api.Repository;

namespace CoffeeShop.ShoppingCart.Api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartCosmosDbRepository cartCosmosDbRepository;
        private readonly ICartRedisCacheRepository cartRedisCacheRepository;

        public CartService(ICartCosmosDbRepository cartCosmosDbRepository, ICartRedisCacheRepository cartRedisCacheRepository)
        {
            this.cartCosmosDbRepository = cartCosmosDbRepository;
            this.cartRedisCacheRepository = cartRedisCacheRepository;
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            var cart = await cartRedisCacheRepository.GetCartAsync(userId);
            if (cart != null)
            {
                return cart;
            }

            cart = await cartCosmosDbRepository.GetCartAsync(userId);
            if (cart != null)
            {
                await cartRedisCacheRepository.AddOrUpdateCartAsync(userId, cart);
            }

            return cart;
        }

        public async Task AddOrUpdateCartAsync(Cart cart)
        {
            await cartCosmosDbRepository.AddOrUpdateCartAsync(cart);
            await cartRedisCacheRepository.AddOrUpdateCartAsync(cart.UserId, cart);
        }

        public async Task RemoveCartAsync(int userId)
        {
            await cartRedisCacheRepository.RemoveCartAsync(userId);
            await cartCosmosDbRepository.RemoveCartAsync(userId);
        }
    }
}
