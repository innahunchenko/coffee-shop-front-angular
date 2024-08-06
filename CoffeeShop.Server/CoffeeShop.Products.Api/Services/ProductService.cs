using AutoMapper;
using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using GrpcCacheClient;
using static GrpcCacheClient.CacheService;
using KeyValuePair = GrpcCacheClient.KeyValuePair;


namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly CacheServiceClient cacheClient;

        public ProductService(IProductRepository productRepository,
                              IMapper mapper,
                              CacheServiceClient cacheClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheClient = cacheClient;
        }

        public async Task<PaginatedList<ProductDto?>> GetProductsAsync(Filter filter, int pageNumber, int pageSize)
        {
            var indexKey = GetIndexKeyFromFilter(filter);
            var totalKey = $"{indexKey}:total";

            var cachedProducts = await SearchProductsInCacheAsync(filter);
            int totalProducts;

            if (cachedProducts.Any())
            {
                var cachedTotalProducts = await cacheClient.GetValueAsync(new GetValueRequest() { Key = totalKey });
                totalProducts = string.IsNullOrEmpty(cachedTotalProducts.Value) ? 0 : int.Parse(cachedTotalProducts.Value);
                Console.WriteLine($"products from cache, total {totalProducts}");
                var mappedProducts = mapper.Map<List<ProductDto?>>(cachedProducts);
                return new PaginatedList<ProductDto?>(mappedProducts, totalProducts, pageNumber, pageSize);
            }

            var productsFromDb = await GetProductsFromDbAsync(filter, pageNumber, pageSize);
            totalProducts = await productRepository.GetTotalProductsAsync(filter);

            if (productsFromDb.Any())
            {
                await CacheProductsAsync(productsFromDb.ToList(), filter);
                await cacheClient.SetValueAsync(new SetValueRequest() { Key = totalKey, Value = totalProducts.ToString() });
                Console.WriteLine($"products from db, total {totalProducts}");
                var mappedProducts = mapper.Map<List<ProductDto?>>(productsFromDb);
                return new PaginatedList<ProductDto?>(mappedProducts, totalProducts, pageNumber, pageSize);
            }

            return default;
        }

        private string GetIndexKeyFromFilter(Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                return $"index:product:name:{filter.Search.ToLower()}";
            }
            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                return $"index:product:subcategory:{filter.Subcategory.ToLower()}";
            }
            if (!string.IsNullOrEmpty(filter.Category))
            {
                return $"index:product:category:{filter.Category.ToLower()}";
            }
            return "index:product:all";
        }


        public async Task<List<ProductDto?>> SearchProductsInCacheAsync(Filter filter)
        {
            var productKeys = new List<string>();

            switch (filter)
            {
                case { Search: { Length: > 0 } }:
                    var searchKeys = await GetProductKeysAsync($"index:product:name:{filter.Search.ToLower()}");
                    productKeys.AddRange(searchKeys);
                    break;

                case { Subcategory: { Length: > 0 } }:
                    var subcategoryKeys = await GetProductKeysAsync($"index:product:subcategory:{filter.Subcategory.ToLower()}");
                    productKeys.AddRange(subcategoryKeys);
                    break;

                case { Category: { Length: > 0 } }:
                    var categoryKeys = await GetProductKeysAsync($"index:product:category:{filter.Category.ToLower()}");
                    productKeys.AddRange(categoryKeys);
                    break;

                default:
                    var allKeys = await GetProductKeysAsync("index:product:all");
                    productKeys.AddRange(allKeys);
                    break;
            }

            return await GetProductsFromCacheAsync(productKeys);
        }

        private async Task<IEnumerable<string>> GetProductKeysAsync(string setKey)
        {
            var response = await cacheClient.GetIndexMembersAsync(new GetIndexMembersRequest { IndexKey = setKey });
            return response.Members.ToList();
        }

        private async Task<List<ProductDto?>> GetProductsFromCacheAsync(IEnumerable<string> productKeys)
        {
            var products = new List<ProductDto?>();

            foreach (var key in productKeys)
            {
                var entryDictionary = await cacheClient.GetHashAllAsync(new GetHashAllRequest { HashKey = key });
                if (entryDictionary != null && entryDictionary.Entries.Count > 0)
                {
                    var productDto = mapper.Map<ProductDto>(entryDictionary.Entries);
                    products.Add(productDto);
                }
            }

            return products;
        }

        private async Task<IEnumerable<ProductDto?>> GetProductsFromDbAsync(Filter filter, int pageNumber, int pageSize)
        {
            var productsFromDb = await productRepository.GetProductsAsync(filter, pageNumber, pageSize);
            return mapper.Map<List<ProductDto?>>(productsFromDb);
        }

        private async Task CacheProductsAsync(List<ProductDto> products, Filter filter)
        {
            var tasks = products.Select(product => AddProductToCacheAsync(product, filter));
            await Task.WhenAll(tasks);
        }

        public async Task AddProductToCacheAsync(ProductDto product, Filter filter)
        {
            var productKey = $"product:{product.Id}";
            try
            {
                var entries = mapper.Map<IEnumerable<KeyValuePair>>(product);

                // Check if the product already exists in the cache
                var productInCache = await cacheClient.GetHashAllAsync(new GetHashAllRequest { HashKey = productKey });

                // If the product does not exist in the cache, add it
                if (productInCache?.Entries == null || !productInCache.Entries.Any())
                {
                    var request = new SetHashDataBatchRequest
                    {
                        Key = productKey,
                        Entries = { entries }
                    };

                    await cacheClient.SetHashDataBatchAsync(request);
                }

                // Add the product to the relevant indexes based on the filter
                await AddProductToIndexesAsync(product, filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task AddProductToIndexesAsync(ProductDto product, Filter filter)
        {
            var productKey = $"product:{product.Id}";
            var indexKeys = new List<string> { $"index:product:name:{product.Name.ToLower()}" };

            // Add indexes based on the filter
            switch (filter)
            {
                case { Subcategory: { Length: > 0 } subcategory }:
                    indexKeys.Add($"index:product:subcategory:{subcategory.ToLower()}");
                    break;

                case { Category: { Length: > 0 } category }:
                    indexKeys.Add($"index:product:category:{category.ToLower()}");
                    break;
                default:
                    indexKeys.Add("index:product:all");
                    break;
            }

            // Add the product to the corresponding indexes
            var tasks = indexKeys.Select(async indexKey =>
            {
                var setAddRequest = new SetAddRequest
                {
                    IndexKey = indexKey,
                    ItemKey = productKey
                };

                await cacheClient.SetAddAsync(setAddRequest).ResponseAsync;

                var totalKey = $"{indexKey}:total";
                var currentTotalResponse = await cacheClient.GetValueAsync(new GetValueRequest { Key = totalKey });
                var newTotal = string.IsNullOrEmpty(currentTotalResponse.Value) ? 1 : int.Parse(currentTotalResponse.Value) + 1;
                await cacheClient.SetValueAsync(new SetValueRequest { Key = totalKey, Value = newTotal.ToString() });
            });

            await Task.WhenAll(tasks);
        }
    }
}
