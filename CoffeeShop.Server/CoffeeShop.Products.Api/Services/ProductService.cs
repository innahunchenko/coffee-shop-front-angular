using AutoMapper;
using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using GrpcCacheClient;
using Newtonsoft.Json;
using static GrpcCacheClient.CacheService;
using KeyValuePair = GrpcCacheClient.KeyValuePair;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly CacheServiceClient cacheClient;

        const string CATEGORIES_KEY = "KEY:CATEGORIES";

        public ProductService(IProductRepository productRepository,
                              IMapper mapper,
                              CacheServiceClient cacheClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheClient = cacheClient;
        }

        public async Task<List<CategoryDto?>> GetCategoriesAsync()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"GetCategoriesAsync: Start (Thread {threadId})");

            var cachedCategories = await GetCachedCategoriesAsync();
            if (cachedCategories.Any())
            {
                Console.WriteLine($"GetCategoriesAsync cache: End (Thread {threadId})");
                return cachedCategories;
            }

            var categoriesFromDb = await GetCategoriesFromDbAsync();
            if (categoriesFromDb.Any())
            {
                await CacheCategoriesAsync(categoriesFromDb);
                Console.WriteLine($"GetCategoriesAsync db: End (Thread {threadId})");
                return categoriesFromDb;
            }

            return new List<CategoryDto?>();
        }

        private async Task<List<CategoryDto?>> GetCachedCategoriesAsync()
        {
            var cachedValues = await cacheClient.GetAllAsync(new GetAllRequest { HashKey = CATEGORIES_KEY });
            return cachedValues.Entries.Select(entry => JsonConvert.DeserializeObject<CategoryDto>(entry.Data)).ToList();
        }

        private async Task<List<CategoryDto?>> GetCategoriesFromDbAsync()
        {
            var categoriesFromDb = await productRepository.GetMainCategoriesWithSubcategoriesAsync();
            return mapper.Map<List<CategoryDto?>>(categoriesFromDb);
        }

        private async Task CacheCategoriesAsync(List<CategoryDto?> categories)
        {
            var hashEntries = categories.Select(category => new KeyValuePair
            {
                Id = category?.Name,
                Data = JsonConvert.SerializeObject(category)
            }).ToList();

            var request = new SetDataBatchRequest
            {
                Key = CATEGORIES_KEY,
                Entries = { hashEntries }
            };

            await cacheClient.SetDataBatchAsync(request);
        }

        public async Task<IEnumerable<ProductDto?>> GetProductsAsync(Filter filter)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"GetProductsAsync: Start (Thread {threadId})");

            var cachedProducts = await SearchProductsInCacheAsync(filter);
            if (cachedProducts.Any())
            {
                Console.WriteLine($"GetProductsAsync cache: End (Thread {threadId})");
                return cachedProducts;
            }

            var productsFromDb = await GetProductsFromDbAsync(filter);
            if (productsFromDb.Any())
            {
                await CacheProductsAsync(productsFromDb, filter);
                Console.WriteLine($"GetProductsAsync db: End (Thread {threadId})");
                return productsFromDb;
            }

            return Enumerable.Empty<ProductDto?>();
        }

        public async Task<List<ProductDto?>> SearchProductsInCacheAsync(Filter filter)
        {
            var productKeys = new HashSet<string>();

            switch (filter)
            {
                case { Search: { Length: > 0 } }:
                    var searchKeys = await GetProductKeysAsync($"index:product:name:{filter.Search.ToLower()}");
                    productKeys.UnionWith(searchKeys);
                    break;

                case { Subcategory: { Length: > 0 } }:
                    var subcategoryKeys = await GetProductKeysAsync($"index:product:subcategory:{filter.Subcategory.ToLower()}");
                    productKeys.UnionWith(subcategoryKeys);
                    break;

                case { Category: { Length: > 0 } }:
                    var categoryKeys = await GetProductKeysAsync($"index:product:category:{filter.Category.ToLower()}");
                    productKeys.UnionWith(categoryKeys);
                    break;

                default:
                    var allKeys = await GetProductKeysAsync("index:product:all");
                    productKeys.UnionWith(allKeys);
                    break;
            }

            return await GetProductsFromCacheAsync(productKeys);
        }

        private async Task<HashSet<string>> GetProductKeysAsync(string setKey)
        {
            var response = await cacheClient.GetIndexMembersAsync(new GetIndexMembersRequest { IndexKey = setKey });
            return response.Members.ToHashSet();
        }

        private async Task<List<ProductDto?>> GetProductsFromCacheAsync(HashSet<string> productKeys)
        {
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

        private async Task<List<ProductDto?>> GetProductsFromDbAsync(Filter filter)
        {
            var productsFromDb = await productRepository.GetProductsAsync(filter);
            return mapper.Map<List<ProductDto?>>(productsFromDb);
        }

        private async Task CacheProductsAsync(List<ProductDto> products, Filter filter)
        {
            var tasks = products.Select(product => AddProductAsync(product, filter));
            await Task.WhenAll(tasks);
        }

        public async Task AddProductAsync(ProductDto product, Filter filter)
        {
            try
            {
                var entries = mapper.Map<IEnumerable<KeyValuePair>>(product);
                var request = new SetDataBatchRequest
                {
                    Key = $"product:{product.Id}",
                    Entries = { entries }
                };

                await cacheClient.SetDataBatchAsync(request);
                await AddProductToIndexAsync(product, filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task AddProductToIndexAsync(ProductDto product, Filter filter)
        {
            var indexKey = filter switch
            {
                { Search: { Length: > 0 } } => $"index:product:name:{product.Name.ToLower()}",
                { Subcategory: { Length: > 0 } } => $"index:product:subcategory:{product.SubcategoryName.ToLower()}",
                { Category: { Length: > 0 } } => $"index:product:category:{product.CategoryName.ToLower()}",
                _ => "index:product:all"
            };

            await cacheClient.SetAddAsync(new SetAddRequest
            {
                IndexKey = indexKey,
                ItemKey = $"product:{product.Id}"
            });
        }
    }
}
