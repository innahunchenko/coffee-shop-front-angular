using CoffeeShop.ShoppingCart.Api.Models;
using Microsoft.Azure.Cosmos;

namespace CoffeeShop.ShoppingCart.Api.Repository
{
    public class CartCosmosDbRepository : ICartCosmosDbRepository
    {
        private readonly Container _container;

        public CartCosmosDbRepository(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task AddOrUpdateCartAsync(Cart cart)
        {
            await _container.UpsertItemAsync(cart, new PartitionKey(cart.UserId));
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = _container.GetItemQueryIterator<Cart>(query);
            if (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                return result.FirstOrDefault();
            }

            return null;
        }

        public async Task RemoveCartAsync(int userId)
        {
            var partitionKey = new PartitionKey(userId);
            await _container.DeleteItemAsync<Cart>(userId.ToString(), partitionKey);
        }
    }
}
