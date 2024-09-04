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
            var products = productsFromDb.ToList();
            await AddProductsToCacheAsync(products);

            await AddProductsToIndexAsync(index, products);
            await cacheClient.SetValueAsync(new SetValueRequest { Key = totalKey, Value = totalProducts.ToString() });
            Console.WriteLine($"Products cached, total {totalProducts}");

            return new PaginatedList<ProductDto?>(mapper.Map<List<ProductDto?>>(productsFromDb), totalProducts, pageNumber, pageSize);
        }

        private async Task<PaginatedList<ProductDto?>?> GetProductsFromCacheAsync(string index, string totalKey, int pageNumber, int pageSize)
        {
            var response = await cacheClient.GetIndexMembersAsync(new GetIndexMembersRequest { IndexKey = index });
            var productKeys = response.Members.ToList();

            var cachedProducts = new List<ProductDto?>();

            foreach (var key in productKeys)
            {
                var entryDictionary = await cacheClient.GetHashAllAsync(new GetHashAllRequest { HashKey = key });
                if (entryDictionary != null && entryDictionary.Entries.Count > 0)
                {
                    var productDto = mapper.Map<ProductDto>(entryDictionary.Entries);
                    cachedProducts.Add(productDto);
                }
            }

            if (!cachedProducts.Any())
            {
                return null;
            }

            var cachedTotalProducts = await cacheClient.GetValueAsync(new GetValueRequest { Key = totalKey });
            int totalProducts = string.IsNullOrEmpty(cachedTotalProducts.Value) ? 0 : int.Parse(cachedTotalProducts.Value);

            var mappedProducts = mapper.Map<List<ProductDto?>>(cachedProducts);
            return new PaginatedList<ProductDto?>(mappedProducts, totalProducts, pageNumber, pageSize);
        }

        private async Task AddProductsToCacheAsync(List<ProductDto?> products)
        {
            var productTasks = products.Select(async product =>
            {
                try
                {
                    var productKey = $"product:{product.Id}";
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error caching product: {ex}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to cache product with ID: {product?.Id}. Exception: {ex.Message}");
                }
            });

            var taskArray = productTasks.ToArray();
            try
            {
                await Task.WhenAll(taskArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"One or more product caching operations failed: {ex.Message}");
            }

            if (taskArray.Any(t => t.IsFaulted))
            {
                Console.WriteLine("One or more tasks failed during product caching.");
            }
        }

        private async Task AddProductsToIndexAsync(string index, IEnumerable<ProductDto> products)
        {
            var productTasks = products.Select(async product =>
            {
                var productKey = $"product:{product.Id}";
                try
                {
                    var setAddRequest = new SetAddRequest
                    {
                        IndexKey = index,
                        ItemKey = productKey
                    };

                    await cacheClient.SetAddAsync(setAddRequest);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to add indices for {productKey}. Exception: {ex.Message}");
                }
            });

            var taskArray = productTasks.ToArray();
            try
            {
                await Task.WhenAll(taskArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"One or more add indecies operations failed: {ex.Message}");
            }

            if (taskArray.Any(t => t.IsFaulted))
            {
                Console.WriteLine("One or more tasks failed during adding indices.");
            }
        }
    }
}
