using GrpcCache;
using NRediSearch;
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
            string connectionString = configuration.GetValue<string>("CacheSettings:RedisConnectionString") ?? "";
            var defaultExpiry = configuration.GetValue<double>("CacheSettings:DefaultCacheDurationMinutes");
            var expirationTime = DateTimeOffset.Now.AddMinutes(defaultExpiry);
            expiration = expirationTime.DateTime.Subtract(DateTime.Now);
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            db = connectionMultiplexer.GetDatabase();
            server = connectionMultiplexer.GetServer(connectionMultiplexer.Configuration);
        }

        public async Task<IDictionary<string, string>> GetAllAsync(string hashKey)
        {
            var values = await db.HashGetAllAsync(hashKey);
            if (values.Length > 0)
            {
                return values.ToDictionary(e => (string)e.Name, e => (string)e.Value);
            }

            return new Dictionary<string, string>();
        }

        public async Task<string> GetDataAsync(string key, string id)
        {
            var value = await db.HashGetAsync(key, id);
            if (!value.IsNullOrEmpty)
            {
                return value;
            }

            return string.Empty;
        }

        public async Task SetDataAsync(string key, string id, string data)
        {
            await db.HashSetAsync(key, [new HashEntry(id, data)]);
            await db.KeyExpireAsync(key, expiration);
        }

        public async Task SetDataAsync(string key, IEnumerable<HashEntry> hashEntries)
        {
            await db.HashSetAsync(key, hashEntries.ToArray());
            await db.KeyExpireAsync(key, expiration);
        }

        public IEnumerable<string> GetHashKeys(string pattern)
        {
            var keys = server.Keys(pattern: pattern);
            return keys.Select(k => k.ToString());
        }

        public async Task<bool> RemoveDataAsync(string key, string id)
        {
            var isDeleted = await db.HashDeleteAsync(key, id);
            return isDeleted;
        }

        public async Task<bool> RemoveHashAsync(string key)
        {
            return await db.KeyDeleteAsync(key);
        }

        public async Task SetAddAsync(string indexKey, string productKey)
        {
            await db.SetAddAsync(indexKey, productKey);
        }

        public async Task<RedisValue[]> SetMembersAsync(string setKey)
        {
            return await db.SetMembersAsync(setKey);
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request)
        {
            var rediSearchClient = new Client(request.IndexName, db);

            var query = new Query(request.Query).Limit(0, request.Limit);

            var searchResult = await Task.Run(() => rediSearchClient.Search(query));

            var response = new SearchResponse();

            foreach (var doc in searchResult.Documents)
            {
                var document = new SearchDocument
                {
                    Id = doc.Id
                };

                var properties = doc.GetProperties();

                foreach (var property in properties)
                {
                    var key = property.Key;
                    var value = doc[key];

                    document.Fields.Add(key, value.ToString());
                }

                response.Documents.Add(document);
            }

            return response;
        }

        public IndexExistsResponse IndexExists(IndexExistsRequest request)
        {
            var result = server.Execute("FT._LIST");
            var indexNames = (RedisResult[])result;
            var indexExists = indexNames.Any(index => index.ToString() == request.IndexName);
            return new IndexExistsResponse { Exists = indexExists };
        }

        public void CreateIndex(string indexName, IEnumerable<Property> properties)
        {
            var client = new Client(indexName, db);
            var schema = new Schema();

            foreach (var property in properties)
            {
                schema.AddTextField(property.Name, 1.0);
            }

            client.CreateIndex(schema, new Client.ConfiguredIndexOptions());
        }

        public void AddDocument(string indexName, string id, IEnumerable<Property> properties)
        {
            var client = new Client(indexName, db);
            var doc = new Document(id);

            foreach (var property in properties)
            {
                doc.Set(property.Name, property.Value);
            }

            client.AddDocument(doc);
        }
    }
}
