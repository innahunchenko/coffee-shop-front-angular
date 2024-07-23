namespace CoffeeShop.Cache.Api.Services
{
    public interface IRedisCacheService
    {
        Task<string> GetCachedDataAsync(string cacheKey);
        Task SetCachedDataAsync(string cacheKey, string data);
        Task SetCachedDataAsync(string cacheKey, string data, TimeSpan expiry);
        Task AddToSetAsync(string setKey, string value);
        Task<IEnumerable<string>> GetAllKeysAsync(string setKey);
        Task<IEnumerable<string>> GetAllCachedDataAsync(string setKey);
        Task InvalidateCacheAsync(string pattern);
    }
}
