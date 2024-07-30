using GrpcCache;
using StackExchange.Redis;

namespace CoffeeShop.Cache.Api.Repository
{
    public interface IRedisCacheRepository
    {
        Task<IDictionary<string, string>> GetAllAsync(string hashKey);
        Task<string> GetDataAsync(string key, string id);
        Task SetDataAsync(string key, string id, string data);
        Task SetDataAsync(string key, IEnumerable<HashEntry> hashEntries);
        Task<bool> RemoveDataAsync(string key, string id);
        Task<bool> RemoveHashAsync(string key);
        IEnumerable<string> GetHashKeys(string key);
        Task SetAddAsync(string indexKey, string productKey);
        Task<RedisValue[]> SetMembersAsync(string setKey);
        void CreateIndex(string indexName, IEnumerable<Property> properties);
        void AddDocument(string indexName, string id, IEnumerable<Property> properties);
        Task<SearchResponse> SearchAsync(SearchRequest request);
        IndexExistsResponse IndexExists(IndexExistsRequest request);

        //Task<string> GetCachedDataAsync(string cacheKey);
        //Task SetCachedDataAsync(string cacheKey, string data);
        //Task SetCachedDataAsync(string cacheKey, string data, TimeSpan expiry);
        //Task AddToSetAsync(string setKey, string value);
        //Task<IEnumerable<string>> GetAllKeysAsync(string setKey);
        //Task InvalidateCacheAsync(string pattern);
    }
}
