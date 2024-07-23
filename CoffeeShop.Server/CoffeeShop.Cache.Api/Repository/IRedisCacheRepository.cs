namespace CoffeeShop.Cache.Api.Repository
{
    public interface IRedisCacheRepository
    {
        Task<string> GetCachedDataAsync(string cacheKey);
        Task SetCachedDataAsync(string cacheKey, string data);
        Task SetCachedDataAsync(string cacheKey, string data, TimeSpan expiry);
        Task AddToSetAsync(string setKey, string value);
        Task<IEnumerable<string>> GetAllKeysAsync(string setKey);
        Task InvalidateCacheAsync(string pattern);
    }
}
