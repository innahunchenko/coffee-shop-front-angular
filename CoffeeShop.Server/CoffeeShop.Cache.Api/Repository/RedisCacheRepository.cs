using GrpcCache;
using StackExchange.Redis;

namespace CoffeeShop.Cache.Api.Repository
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly IDatabase db;
        private readonly IServer server;
        private readonly TimeSpan expiration;

        public RedisCacheRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>("CacheSettings:RedisConnectionString");
            var defaultExpiry = configuration.GetValue<double>("CacheSettings:DefaultCacheDurationMinutes");
            var expirationTime = DateTimeOffset.Now.AddMinutes(defaultExpiry);
            expiration = expirationTime.DateTime.Subtract(DateTime.Now);
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            db = connectionMultiplexer.GetDatabase();
            server = connectionMultiplexer.GetServer(connectionMultiplexer.Configuration);
        }

        public async Task<IDictionary<string, string>> GetHashAllAsync(string hashKey)
        {
            var values = await db.HashGetAllAsync(hashKey);
            if (values.Length > 0)
            {
                return values.ToDictionary(e => (string)e.Name, e => (string)e.Value);
            }

            return new Dictionary<string, string>();
        }

        public async Task<string?> GetHashDataAsync(string key, string id)
        {
            var value = await db.HashGetAsync(key, id);
            if (!value.IsNullOrEmpty)
            {
                return value;
            }

            return string.Empty;
        }

        public async Task HashSetDataAsync(string key, string id, string data)
        {
            await db.HashSetAsync(key, [new HashEntry(id, data)]);
            await db.KeyExpireAsync(key, expiration);
        }

        public async Task HashSetDataAsync(string key, IEnumerable<HashEntry> hashEntries)
        {
            await db.HashSetAsync(key, hashEntries.ToArray());
            await db.KeyExpireAsync(key, expiration);
        }

        public IEnumerable<string> GetHashKeys(string pattern)
        {
            var keys = server.Keys(pattern: pattern);
            return keys.Select(k => k.ToString());
        }

        public async Task<bool> RemoveHashDataAsync(string key, string id)
        {
            var isDeleted = await db.HashDeleteAsync(key, id);
            return isDeleted;
        }

        public async Task<bool> RemoveHashKeyAsync(string key)
        {
            return await db.KeyDeleteAsync(key);
        }

        public async Task SetIndexAsync(string indexKey, string productKey)
        {
            await db.SetAddAsync(indexKey, productKey);
        }

        public async Task<RedisValue[]> GetIndexMembers(string indexKey)
        {
            return await db.SetMembersAsync(indexKey);
        }

        public Task<bool> SetValueAsync(string key, string value)
        {
            db.StringSet(key, value);
            return Task.FromResult(true);
        }

        public async Task<string?> GetValueAsync(string key)
        {
            try
            {
                return await db.StringGetAsync(key);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
