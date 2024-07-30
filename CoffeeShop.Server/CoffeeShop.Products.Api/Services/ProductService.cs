using AutoMapper;
using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using GrpcCacheClient;
using Newtonsoft.Json;
using System.Text;
using static GrpcCacheClient.CacheService;
using KeyValuePair = GrpcCacheClient.KeyValuePair;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly CacheServiceClient cacheClient;
        //private static readonly SemaphoreSlim categorySemaphore = new SemaphoreSlim(1, 1);
        //private static readonly SemaphoreSlim productSemaphore = new SemaphoreSlim(1, 1);
        private static readonly object indexCreationLock = new object();

        private static readonly string CATEGORY_KEY = "CATEGORY_KEY";
        private static readonly string PRODUCT_INDEX = "PRODUCT_INDEX";
        private static readonly string CATEGORY_INDEX = "CATEGORY_INDEX";

        public ProductService(IProductRepository productRepository,
                              IMapper mapper,
                              CacheServiceClient cacheClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheClient = cacheClient;
        }

        private void InitializeIndexes()
        {
            lock (indexCreationLock)
            {
                CreateIndex<ProductDto>(PRODUCT_INDEX);
                CreateIndex<CategoryDto>(CATEGORY_INDEX);
            }
        }

        private List<Property> GetPropertiesForType<T>()
        {
            var properties = new List<Property>();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                properties.Add(new Property
                {
                    Name = propertyInfo.Name,
                    Value = propertyInfo.PropertyType.Name.ToLower()
                });
            }

            return properties;
        }

        public void CreateIndex<T>(string indexName)
        {
            var indexExistsRequest = new IndexExistsRequest { IndexName = indexName };
            var indexExistsResponse = cacheClient.IndexExists(indexExistsRequest);

            if (!indexExistsResponse.Exists)
            {
                var properties = GetPropertiesForType<T>();

                var createIndexRequest = new CreateIndexRequest
                {
                    IndexName = indexName
                };

                createIndexRequest.Properties.AddRange(properties);

                cacheClient.CreateIndex(createIndexRequest);
            }
        }

        public async Task<List<CategoryDto?>> GetCategoriesAsync()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"GetCategoriesAsync: Start (Thread {threadId})");

            var result = new List<CategoryDto>();
            var cachedValues = await cacheClient.GetAllAsync(new GetAllRequest() { HashKey = CATEGORY_KEY });
            var cachedCategories = cachedValues.Entries.Select(entry => JsonConvert.DeserializeObject<CategoryDto>(entry.Data)).ToList();

            if (cachedCategories.Any())
            {
                Console.WriteLine($"GetCategoriesAsync cache: End (Thread {threadId})");
                return cachedCategories;
            }

            var categoriesFromDb = mapper.Map<List<CategoryDto>>(await productRepository.GetMainCategoriesWithSubcategoriesAsync());

            if (categoriesFromDb.Any())
            {
                var hashEntries = categoriesFromDb.Select(category => new KeyValuePair
                {
                    Id = category.Name,
                    Data = JsonConvert.SerializeObject(category)
                }).ToList();

                var request = new SetDataBatchRequest
                {
                    Key = CATEGORY_KEY,
                    Entries = { hashEntries }
                };

                await cacheClient.SetDataBatchAsync(request);

                foreach (var category in categoriesFromDb)
                {
                    var addDocumentRequest = new AddDocumentRequest
                    {
                        IndexName = CATEGORY_INDEX,
                        Id = $"category:{category.Name}"
                    };

                    addDocumentRequest.Properties.Add(new Property
                    {
                        Name = category.Name,
                        Value = category.Subcategories.ToString()
                    });
                    await cacheClient.AddDocumentAsync(addDocumentRequest);
                }

                Console.WriteLine($"GetCategoriesAsync db: End (Thread {threadId})");
                return categoriesFromDb;

            }

            return result;
        }

        public async Task AddProductToCacheAsync(ProductDto product)
        {
            var entries = mapper.Map<IEnumerable<KeyValuePair>>(product);
            var request = new SetDataBatchRequest
            {
                Key = $"product:{product.Id}"
            };

            request.Entries.AddRange(entries);
            await cacheClient.SetDataBatchAsync(request);

            var addDocumentRequest = new AddDocumentRequest
            {
                IndexName = PRODUCT_INDEX,
                Id = $"product:{product.Id}"
            };

            foreach (var entry in entries)
            {
                addDocumentRequest.Properties.Add(new Property
                {
                    Name = entry.Id,
                    Value = entry.Data.ToString()
                });
            }

            await cacheClient.AddDocumentAsync(addDocumentRequest);
        }

        public async Task<IEnumerable<ProductDto?>> GetProductsAsync(Filter filter)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"GetProductsAsync: Start (Thread {threadId})");
            var result = new List<ProductDto>();

            var cachedProducts = await SearchProductsAsync(filter);
            if (cachedProducts.Any())
            {
                Console.WriteLine($"GetProductsAsync cache: End (Thread {threadId})");
                return cachedProducts;
            }

            var productsFromDb = await productRepository.GetProductsAsync(filter);
            var dtoProducts = mapper.Map<List<ProductDto>>(productsFromDb);

            if (dtoProducts.Any())
            {
                var tasks = dtoProducts.Select(AddProductToCacheAsync);
                await Task.WhenAll(tasks);
                Console.WriteLine($"GetProductsAsync db: End (Thread {threadId})");
                return dtoProducts;
            }

            return result;
        }

        public async Task<List<ProductDto?>> SearchProductsAsync(Filter filter)
        {
            var searchQuery = BuildSearchQuery(filter);
            InitializeIndexes();
            var searchResponse = await cacheClient.SearchAsync(new SearchRequest
            {
                IndexName = PRODUCT_INDEX,
                Query = searchQuery,
                Limit = 1000
            });

            var productKeys = searchResponse.Documents
                .Select(doc => doc.Id)
                .ToList();

            var products = new List<ProductDto?>();

            foreach (var key in productKeys)
            {
                var entryDictionary = await cacheClient.GetAllAsync(new GetAllRequest { HashKey = key });

                if (entryDictionary != null && entryDictionary.Entries.Count > 0)
                {
                    var productDto = mapper.Map<ProductDto>(entryDictionary.Entries);
                    products.Add(productDto);
                }
            }

            return products;
        }

        private string BuildSearchQuery(Filter filter)
        {
            var query = new StringBuilder();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query.Append($"@Name:{filter.Search}");
            }

            if (!string.IsNullOrEmpty(filter.Category))
            {
                if (query.Length > 0)
                {
                    query.Append(" ");
                }
                query.Append($"@CategoryName:{filter.Category}");
            }

            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                if (query.Length > 0)
                {
                    query.Append(" ");
                }
                query.Append($"@SubcategoryName:{filter.Subcategory}");
            }

            return string.IsNullOrEmpty(query.ToString()) ? "*" : query.ToString();
        }
    }
}
