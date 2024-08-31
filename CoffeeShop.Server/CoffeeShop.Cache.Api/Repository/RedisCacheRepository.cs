using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

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
            expiration = TimeSpan.FromMinutes(configuration.GetValue<double>("CacheSettings:DefaultCacheDurationMinutes"));
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            db = connectionMultiplexer.GetDatabase();
            server = connectionMultiplexer.GetServer(connectionMultiplexer.Configuration);
            redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { connectionMultiplexer });
        }

        private async Task<bool> AcquireLockAsync(string key, Func<Task> action, int maxRetries = 5, TimeSpan? retryDelay = null)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            int retries = 0;
            retryDelay ??= TimeSpan.FromMilliseconds(200);

            while (retries < maxRetries)
            {
                retries++;
                try
                {
                    await using (var redLock = await redLockFactory.CreateLockAsync(key, expiration))
                    {
                        if (redLock.IsAcquired)
                        {
                            Console.WriteLine($"Thread {threadId} acquired lock for key {key}.");
                            await action();
                            Console.WriteLine($"Thread {threadId} released lock for key {key}. {redLock.Status}");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Thread {threadId} could not acquire lock for key {key}. Retry {retries}.");
                            await Task.Delay(retryDelay.Value);
                        }
                    }
                }
                catch (RedisException ex)
                {
                    Console.WriteLine($"Thread {threadId} encountered Redis error performing action with key {key}: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Thread {threadId} encountered error performing action with key {key}: {ex.Message}");
                    return false;
                }
            }

            Console.WriteLine($"Thread {threadId} exhausted retries to acquire lock for key {key}.");
            return false;
        }

        public async Task<IDictionary<string, string>> GetHashAllAsync(string hashKey)
        {
            try
            {
                var values = await db.HashGetAllAsync(hashKey);
                return values.ToDictionary(e => (string)e.Name, e => (string)e.Value);
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
            await AcquireLockAsync(key, async () =>
            {
                var existingValue = await db.HashGetAsync(key, id);
                if (existingValue.HasValue)
                {
                    Console.WriteLine($"Data already exists for key {key} and id {id}. Skipping update.");
                    return;
                }

                await db.HashSetAsync(key, [new HashEntry(id, data)]);
                await db.KeyExpireAsync(key, expiration);
            });
        }

        public async Task HashSetDataAsync(string key, IEnumerable<HashEntry> hashEntries)
        {
            await AcquireLockAsync(key, async () =>
            {
                var existingEntries = await db.HashGetAllAsync(key);
                if (existingEntries.Any())
                {
                    Console.WriteLine($"Data already exists for key {key}. Skipping update.");
                    return;
                }

                await db.HashSetAsync(key, hashEntries.ToArray());
                await db.KeyExpireAsync(key, expiration);
            });
        }

        public Task<IEnumerable<string>> GetHashKeysAsync(string pattern)
        {
            try
            {
                var keys = server.Keys(pattern: pattern).Select(k => k.ToString()).ToList();
                return Task.FromResult<IEnumerable<string>>(keys);
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Error getting keys with pattern {pattern}: {ex.Message}");
                return Task.FromResult(Enumerable.Empty<string>());
            }
        }

        public Task<bool> RemoveHashDataAsync(string key, string id)
        {
            return AcquireLockAsync(key, async () => await db.HashDeleteAsync(key, id));
        }

        public Task<bool> RemoveHashKeyAsync(string key)
        {
            return AcquireLockAsync(key, async () => await db.KeyDeleteAsync(key));
        }

        public async Task SetIndexAsync(string indexKey, string itemKey)
        {
            await AcquireLockAsync(indexKey, async () =>
            {
                var members = await db.SetMembersAsync(indexKey);
                if (members.Contains(itemKey))
                {
                    Console.WriteLine($"Item {itemKey} already exists in index {indexKey}. Skipping update.");
                    return;
                }

                await db.SetAddAsync(indexKey, itemKey);
            });
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
            await AcquireLockAsync(key, async () =>
            {
                await db.StringSetAsync(key, value, expiration);
            });
        }


    }
}
