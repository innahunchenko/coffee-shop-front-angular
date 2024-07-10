using CoffeeShop.ShoppingCart.Api.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoffeeShop.ShoppingCart.Api.Repository
{
    public class CartRedisCacheRepository : ICartRedisCacheRepository
    {
        private readonly IDatabase database;

        public CartRedisCacheRepository(ConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }

        public async Task AddOrUpdateCartAsync(int userId, Cart cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            await database.StringSetAsync(userId.ToString(), json);
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            var json = await database.StringGetAsync(userId.ToString());
            return json.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Cart>(json);
        }

        public async Task RemoveCartAsync(int userId)
        {
            await database.KeyDeleteAsync(userId.ToString());
        }
    }
}
