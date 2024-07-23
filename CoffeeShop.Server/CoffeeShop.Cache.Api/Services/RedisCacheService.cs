using CoffeeShop.Cache.Api.Repository;

namespace CoffeeShop.Cache.Api.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IRedisCacheRepository redisCacheRepository;

        public RedisCacheService(IRedisCacheRepository redisCacheRepository)
        {
            this.redisCacheRepository = redisCacheRepository ?? throw new ArgumentNullException(nameof(redisCacheRepository));
        }

        public async Task<string> GetCachedDataAsync(string cacheKey)
        {
            return await redisCacheRepository.GetCachedDataAsync(cacheKey);
        }

        public async Task SetCachedDataAsync(string cacheKey, string data)
        {
            await redisCacheRepository.SetCachedDataAsync(cacheKey, data);
        }

        public async Task SetCachedDataAsync(string cacheKey, string data, TimeSpan expiry)
        {
            await redisCacheRepository.SetCachedDataAsync(cacheKey, data, expiry);
        }

        public async Task AddToSetAsync(string setKey, string value)
        {
            await redisCacheRepository.AddToSetAsync(setKey, value);
        }

        public async Task<IEnumerable<string>> GetAllKeysAsync(string setKey)
        {
            return await redisCacheRepository.GetAllKeysAsync(setKey);
        }

        public async Task InvalidateCacheAsync(string pattern)
        {
            await redisCacheRepository.InvalidateCacheAsync(pattern);
        }

        public async Task<IEnumerable<string>> GetAllCachedDataAsync(string setKey)
        {
            var keys = await GetAllKeysAsync(setKey);
            var tasks = keys.Select(async key => await GetCachedDataAsync(key));
            var results = await Task.WhenAll(tasks);
            return results;//results.Where(result => result != null);
        }
    }
}