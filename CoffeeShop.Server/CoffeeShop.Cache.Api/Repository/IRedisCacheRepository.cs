using StackExchange.Redis;

namespace CoffeeShop.Cache.Api.Repository
{
    public interface IRedisCacheRepository
    {
        Task<IDictionary<string, string>> GetHashAllAsync(string hashKey);
        Task<string?> GetHashDataAsync(string key, string id);
        Task HashSetDataAsync(string key, string id, string data);
        Task HashSetDataAsync(string key, IEnumerable<HashEntry> hashEntries);
        Task<bool> RemoveHashDataAsync(string key, string id);
        Task<bool> RemoveHashKeyAsync(string key);
        IEnumerable<string> GetHashKeys(string key);
        Task SetIndexAsync(string indexKey, string itemKey);
        Task<RedisValue[]> GetIndexMembers(string indexKey);
        Task<bool> SetValueAsync(string key, string value);
        Task<string?> GetValueAsync(string key);
    }
}
