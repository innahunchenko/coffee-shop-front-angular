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

        public static readonly string SubcategoryIndexTemplate = "index:product:subcategory:{0}";
        public static readonly string CategoryIndexTemplate = "index:product:category:{0}";
        public static readonly string ProductNameIndexTemplate = "index:product:name:{0}";
        public static readonly string AllProductsIndexTemplate = "index:product:all";

        public ProductService(IProductRepository productRepository,
                              IMapper mapper,
                              CacheServiceClient cacheClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheClient = cacheClient;
        }

        public async Task<PaginatedList<ProductDto?>?> GetProductsBySubcategoryAsync(string subcategory, int pageNumber, int pageSize)
        {
            return await GetProductsAsync(
                async () =>
                {
                    var products = await productRepository.GetProductsBySubcategoryAsync(subcategory, pageNumber, pageSize);
                    return mapper.Map<IEnumerable<ProductDto?>>(products);
                },
                () => productRepository.GetSubcategoryProductsTotalCountAsync(subcategory),
                SubcategoryIndexTemplate,
                subcategory,
                pageNumber,
                pageSize);
        }

        public async Task<PaginatedList<ProductDto?>?> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize)
        {
            return await GetProductsAsync(
                async () =>
                { 
                    var products = await productRepository.GetProductsByCategoryAsync(category, pageNumber, pageSize);
                    return mapper.Map<IEnumerable<ProductDto?>>(products);
                },
                () => productRepository.GetCategoryProductsTotalCountAsync(category),
                CategoryIndexTemplate,
                category,
                pageNumber,
                pageSize);
        }

        public async Task<PaginatedList<ProductDto?>?> GetProductsByNameAsync(string name, int pageNumber, int pageSize)
        {
            return await GetProductsAsync(
                async () =>
                {
                    var products = await productRepository.GetProductsByProductNameAsync(name, pageNumber, pageSize);
                    return mapper.Map<IEnumerable<ProductDto?>>(products);
                },
                () => productRepository.GetProductNameTotalCountAsync(name),
                ProductNameIndexTemplate,
                name,
                pageNumber,
                pageSize);
        }

        public async Task<PaginatedList<ProductDto?>?> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            return await GetProductsAsync(
                async () =>
                {
                    var products = await productRepository.GetAllProductsAsync(pageNumber, pageSize);
                    return mapper.Map<IEnumerable<ProductDto?>>(products);
                },
                productRepository.GetAllProductsTotalCountAsync,
                AllProductsIndexTemplate,
                string.Empty,
                pageNumber,
                pageSize);
        }

        private async Task<PaginatedList<ProductDto?>?> GetProductsAsync(
            Func<Task<IEnumerable<ProductDto?>>> getProductsFromDbFunc,
            Func<Task<int>> getTotalProductsCountFunc,
            string indexKeyTemplate,
            string filterKey,
            int pageNumber,
            int pageSize)
        {
            var index = string.Format(indexKeyTemplate + ":page:{1}", filterKey.ToLower(), pageNumber);
            var totalKey = $"{index}:total";

            var cachedResult = await GetProductsFromCacheAsync(index, totalKey, pageNumber, pageSize);
            if (cachedResult != null)
            {
                Console.WriteLine($"from cache {cachedResult.TotalPages}, {cachedResult.PageIndex}");
                return cachedResult;
            }

            var productsFromDb = await getProductsFromDbFunc();
            if (!productsFromDb.Any())
            {
                return default;
            }

            int totalProducts = await getTotalProductsCountFunc();

            await CacheProductsAsync(productsFromDb, index, totalKey, totalProducts);

            return new PaginatedList<ProductDto?>(mapper.Map<List<ProductDto?>>(productsFromDb), totalProducts, pageNumber, pageSize);
        }

        private async Task<PaginatedList<ProductDto?>?> GetProductsFromCacheAsync(string index, string totalKey, int pageNumber, int pageSize)
        {
            var cachedProducts = await SearchProductsInCacheAsync(index);
            if (!cachedProducts.Any())
            {
                return null;
            }

            int totalProducts = await GetOrUpdateTotalProductsAsync(cachedProducts, totalKey, index);

            var mappedProducts = mapper.Map<List<ProductDto?>>(cachedProducts);
            return new PaginatedList<ProductDto?>(mappedProducts, totalProducts, pageNumber, pageSize);
        }

        private async Task<int> GetOrUpdateTotalProductsAsync(List<ProductDto?> cachedProducts, string totalKey, string index)
        {
            var cachedTotalProducts = await cacheClient.GetValueAsync(new GetValueRequest { Key = totalKey });
            int totalProducts = string.IsNullOrEmpty(cachedTotalProducts.Value) ? 0 : int.Parse(cachedTotalProducts.Value);

            if (cachedProducts.Count != 0 && totalProducts == 0)
            {
                foreach (var product in cachedProducts)
                {
                    await AddProductToIndexesAsync(product, index);
                }

                totalProducts = await GetTotalProductsFromCacheAsync(totalKey);
            }

            return totalProducts;
        }

        private async Task<int> GetTotalProductsFromCacheAsync(string totalKey)
        {
            var cachedTotalProducts = await cacheClient.GetValueAsync(new GetValueRequest { Key = totalKey });
            return string.IsNullOrEmpty(cachedTotalProducts.Value) ? 0 : int.Parse(cachedTotalProducts.Value);
        }

        private async Task CacheProductsAsync(IEnumerable<ProductDto?> products, string index, string totalKey, int totalProducts)
        {
            await CacheProductsAsync(products.ToList(), index);
            await cacheClient.SetValueAsync(new SetValueRequest { Key = totalKey, Value = totalProducts.ToString() });
            Console.WriteLine($"products cached, total {totalProducts}");
        }

        public async Task<List<ProductDto?>> SearchProductsInCacheAsync(string index)
        {
            var keys = await GetProductKeysAsync(index);
            return await GetProductsFromCacheAsync(keys);
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

        private async Task CacheProductsAsync(List<ProductDto?> products, string index)
        {
            var tasks = products.Select(product => AddProductToCacheAsync(product, index));
            await Task.WhenAll(tasks);
        }

        public async Task AddProductToCacheAsync(ProductDto? product, string index)
        {
            var productKey = $"product:{product?.Id}";
            try
            {
                var entries = mapper.Map<IEnumerable<KeyValuePair>>(product);

                var productInCache = await cacheClient.GetHashAllAsync(new GetHashAllRequest { HashKey = productKey });

                if (productInCache?.Entries == null || !productInCache.Entries.Any())
                {
                    var request = new SetHashDataBatchRequest
                    {
                        Key = productKey,
                        Entries = { entries }
                    };

                    await cacheClient.SetHashDataBatchAsync(request);
                }

                await AddProductToIndexesAsync(product, index);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task AddProductToIndexesAsync(ProductDto? product, string index)
        {
            var productKey = $"product:{product?.Id}";
            var indexKeys = new List<string>
        {
            string.Format(ProductNameIndexTemplate, product?.Name.ToLower()),
            index
        };

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
