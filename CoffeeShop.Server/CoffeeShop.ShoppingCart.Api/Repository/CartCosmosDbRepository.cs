using CoffeeShop.ShoppingCart.Api.Models;
using Microsoft.Azure.Cosmos;

namespace CoffeeShop.ShoppingCart.Api.Repository
{
    public class CartCosmosDbRepository : ICartCosmosDbRepository
    {
        private readonly Container container;

        public CartCosmosDbRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("CosmosDb:ConnectionString");
            var databaseName = configuration.GetValue<string>("CosmosDb:DatabaseName");
            var containerName = configuration.GetValue<string>("CosmosDb:ContainerName");
            var client = new CosmosClient(connectionString);
            container = client.GetContainer(databaseName, containerName);
        }

        public async Task AddOrUpdateCartAsync(Cart cart)
        {
            await container.UpsertItemAsync(cart, new PartitionKey(cart.UserId));
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
                .WithParameter("@userId", userId);

            var iterator = container.GetItemQueryIterator<Cart>(query);
            if (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                var cart = result.FirstOrDefault();
                return cart;
            }

            return null;
        }

        public async Task RemoveCartAsync(int userId)
        {
            var partitionKey = new PartitionKey(userId);
            await container.DeleteItemAsync<Cart>(userId.ToString(), partitionKey);
        }
    }
}