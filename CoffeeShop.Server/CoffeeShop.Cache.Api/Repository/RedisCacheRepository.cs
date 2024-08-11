using StackExchange.Redis;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace CoffeeShop.Cache.Api.Repository
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly IDatabase db;
        private readonly IServer server;
        private readonly TimeSpan expiration;
        private readonly RedLockFactory redLockFactory;

        public RedisCacheRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>("CacheSettings:RedisConnectionString");
            var defaultExpiry = configuration.GetValue<double>("CacheSettings:DefaultCacheDurationMinutes");
            var expirationTime = DateTimeOffset.Now.AddMinutes(defaultExpiry);
            expiration = expirationTime.DateTime.Subtract(DateTime.Now);
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            db = connectionMultiplexer.GetDatabase();
            server = connectionMultiplexer.GetServer(connectionMultiplexer.Configuration);
            redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { connectionMultiplexer });
        }

        public async Task<IDictionary<string, string>> GetHashAllAsync(string hashKey)
        {
            try
            {
                var values = await db.HashGetAllAsync(hashKey);
                if (values.Length > 0)
                {
                    return values.ToDictionary(e => (string)e.Name, e => (string)e.Value);
                }
                return new Dictionary<string, string>();
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting all hash values for key {hashKey}: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }

        public async Task<string?> GetHashDataAsync(string key, string id)
        {
            try
            {
                var value = await db.HashGetAsync(key, id);
                return value.HasValue ? value.ToString() : null;
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting hash data for key {key} and id {id}: {ex.Message}");
                return null;
            }
        }

        public async Task HashSetDataAsync(string key, string id, string data)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        await db.HashSetAsync(key, new[] { new HashEntry(id, data) });
                        await db.KeyExpireAsync(key, expiration);
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for key {key}.");
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error setting hash data for key {key} and id {id}: {ex.Message}");
            }
        }

        public async Task HashSetDataAsync(string key, IEnumerable<HashEntry> hashEntries)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        await db.HashSetAsync(key, hashEntries.ToArray());
                        await db.KeyExpireAsync(key, expiration);
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for key {key}.");
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error setting hash data for key {key}: {ex.Message}");
            }
        }

        public IEnumerable<string> GetHashKeys(string pattern)
        {
            try
            {
                var keys = server.Keys(pattern: pattern);
                return keys.Select(k => k.ToString());
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting keys with pattern {pattern}: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<bool> RemoveHashDataAsync(string key, string id)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        var isDeleted = await db.HashDeleteAsync(key, id);
                        return isDeleted;
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for key {key}.");
                        return false;
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error removing hash data for key {key} and id {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveHashKeyAsync(string key)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        return await db.KeyDeleteAsync(key);
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for key {key}.");
                        return false;
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error removing hash key {key}: {ex.Message}");
                return false;
            }
        }

        public async Task SetIndexAsync(string indexKey, string productKey)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(indexKey, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        await db.SetAddAsync(indexKey, productKey);
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for indexKey {indexKey}.");
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error adding product key {productKey} to index {indexKey}: {ex.Message}");
            }
        }

        public async Task<RedisValue[]> GetIndexMembers(string indexKey)
        {
            try
            {
                return await db.SetMembersAsync(indexKey);
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting members of index {indexKey}: {ex.Message}");
                return Array.Empty<RedisValue>();
            }
        }

        public async Task<string?> GetValueAsync(string key)
        {
            try
            {
                return await db.StringGetAsync(key);
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting value for key {key}: {ex.Message}");
                return null;
            }
        }

        public async Task SetValueAsync(string key, string value)
        {
            try
            {
                await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                {
                    if (redLock.IsAcquired)
                    {
                        await db.StringSetAsync(key, value, expiration);
                    }
                    else
                    {
                        Console.WriteLine($"Could not acquire lock for key {key}.");
                    }
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error setting value for key {key}: {ex.Message}");
            }
        }
    }
}
