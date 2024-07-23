using StackExchange.Redis;

namespace CoffeeShop.Cache.Api.Repository
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly IDatabase redisDatabase;
        private readonly TimeSpan defaultExpiry;

        public RedisCacheRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>("CacheSettings:RedisConnectionString") ?? "";
            int defaultCacheDurationMinutes = configuration.GetValue<int>("CacheSettings:DefaultCacheDurationMinutes");
            defaultExpiry = TimeSpan.FromMinutes(defaultCacheDurationMinutes);
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public async Task<string> GetCachedDataAsync(string cacheKey)
        {
            var redisValue = await redisDatabase.StringGetAsync(cacheKey);
            return redisValue;
            //T? result = JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SetCachedDataAsync(string cacheKey, string data)
        {
            await SetCachedDataAsync(cacheKey, data, defaultExpiry);
        }

        public async Task SetCachedDataAsync(string cacheKey, string data, TimeSpan expiry)
        {
           // var json = JsonConvert.SerializeObject(data);
            await redisDatabase.StringSetAsync(cacheKey, data, expiry);
            await UpdateCacheMetadataAsync(cacheKey);
        }

        private async Task UpdateCacheMetadataAsync(string cacheKey)
        {
            await redisDatabase.StringSetAsync($"metadata:{cacheKey}:lastUpdated", DateTime.UtcNow.ToString("o"));
        }

        public async Task AddToSetAsync(string setKey, string value)
        {
            await redisDatabase.SetAddAsync(setKey, value);
        }

        public async Task<IEnumerable<string>> GetAllKeysAsync(string setKey)
        {
            var keys = await redisDatabase.SetMembersAsync(setKey);
            return keys.Select(k => k.ToString());
        }

        //public async Task<IEnumerable<string>> GetAllKeysAsync(string setKey)
        //{
        //    var server = redisDatabase.Multiplexer.GetServer(redisDatabase.Multiplexer.GetEndPoints().First());
        //    return server.Keys(pattern: $"{setKey}*").Select(key => key.ToString());
        //}

        public async Task InvalidateCacheAsync(string pattern)
        {
            var endpoints = redisDatabase.Multiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = redisDatabase.Multiplexer.GetServer(endpoint);
                var keys = server.Keys(pattern: pattern);
                await redisDatabase.KeyDeleteAsync(keys.ToArray());
            }
        }
    }
}
