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
        private static readonly SemaphoreSlim categorySemaphore = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim productSemaphore = new SemaphoreSlim(1, 1);

        const string CATEGORIES_KEY = "categoriesKey";

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

            var result = new List<CategoryDto>();
            var cachedValues = await cacheClient.GetAllAsync(new GetAllRequest() { HashKey = CATEGORIES_KEY });
            var cachedCategories = cachedValues.Entries.Select(entry => JsonConvert.DeserializeObject<CategoryDto>(entry.Data)).ToList();

            if (cachedCategories.Any())
            {
                Console.WriteLine($"GetCategoriesAsync cache: End (Thread {threadId})");
                return cachedCategories;
            }

            var categoriesFromDb = mapper.Map<List<CategoryDto>>(await productRepository.GetMainCategoriesWithSubcategoriesAsync());

            if (categoriesFromDb.Any())
            {
                await categorySemaphore.WaitAsync();
                try
                {
                    var hashEntries = categoriesFromDb.Select(category => new KeyValuePair
                    {
                        Id = category.Name,
                        Data = JsonConvert.SerializeObject(category)
                    }).ToList();

                    var request = new SetDataBatchRequest
                    {
                        Key = CATEGORIES_KEY,
                        Entries = { hashEntries }
                    };

                    await cacheClient.SetDataBatchAsync(request);
                }
                finally
                {
                    categorySemaphore.Release();
                }

                Console.WriteLine($"GetCategoriesAsync db: End (Thread {threadId})");
                return categoriesFromDb;

            }

            return result;
        }

        public async Task<IEnumerable<ProductDto?>> GetProductsAsync(Filter filter)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"GetProductsAsync: Start (Thread {threadId})");
            var result = new List<ProductDto>();

            var cahcedProducts = await SearchProductsAsync(filter);
            if (cahcedProducts.Any())
            {
                Console.WriteLine($"GetProductsAsync cache: End (Thread {threadId})");
                return cahcedProducts;
            }

            var productsFromDb = await productRepository.GetProductsAsync(filter);
            var dtoProducts = mapper.Map<List<ProductDto>>(productsFromDb);

            if (dtoProducts.Any())
            {
                await productSemaphore.WaitAsync();
                try
                {
                    var tasks = dtoProducts.Select(AddProductAsync);
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    productSemaphore.Release();
                }

                Console.WriteLine($"GetProductsAsync db: End (Thread {threadId})");

                return dtoProducts;
            }

            return result;
        }

        public async Task AddProductAsync(ProductDto product)
        {
            try
            {
                var entries = mapper.Map<IEnumerable<KeyValuePair>>(product);
                var request = new SetDataBatchRequest
                {
                    Key = $"product:{product.Id}"
                };

                request.Entries.AddRange(entries);
                await cacheClient.SetDataBatchAsync(request);
                await cacheClient.SetAddAsync(new SetAddRequest() { IndexKey = $"index:product:name:{product.Name.ToLower()}", ProductKey = $"product:{product.Id}" });
                await cacheClient.SetAddAsync(new SetAddRequest() { IndexKey = $"index:product:category:{product.CategoryName.ToLower()}", ProductKey = $"product:{product.Id}" });
                await cacheClient.SetAddAsync(new SetAddRequest() { IndexKey = $"index:product:subcategory:{product.SubcategoryName.ToLower()}", ProductKey = $"product:{product.Id}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<List<ProductDto?>> SearchProductsAsync(Filter filter)
        {
            var productKeys = new HashSet<string>();

            // Если выбран фильтр по поисковому слову
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var searchKeys = await cacheClient.GetSetMembersAsync(new GetSetMembersRequest
                {
                    SetKey = $"index:product:name:{filter.Search.ToLower()}"
                });
                productKeys.UnionWith(searchKeys.Members);
            }
            else
            {
                // Собираем ключи для подкатегории
                if (!string.IsNullOrEmpty(filter.Subcategory))
                {
                    var subcategoryKeys = await cacheClient.GetSetMembersAsync(new GetSetMembersRequest
                    {
                        SetKey = $"index:product:subcategory:{filter.Subcategory.ToLower()}"
                    });
                    productKeys.UnionWith(subcategoryKeys.Members);
                }

                // Собираем ключи для категории
                if (!string.IsNullOrEmpty(filter.Category))
                {
                    var categoryKeys = await cacheClient.GetSetMembersAsync(new GetSetMembersRequest
                    {
                        SetKey = $"index:product:category:{filter.Category.ToLower()}"
                    });

                    // Для выбора данных только из выбранной категории
                    // Понимаем, что если мы ищем по категории, то нам могут быть интересны только те, которые есть в фильтре категории
                    // Убираем возможные дубликаты и берем только те ключи, которые совпадают с категорией
                    productKeys.UnionWith(categoryKeys.Members);
                }

                // Если ни подкатегория, ни категория не указаны, собираем все данные
                if (string.IsNullOrEmpty(filter.Subcategory) && string.IsNullOrEmpty(filter.Category))
                {
                    var allKeys = await cacheClient.GetHashKeysAsync(new GetHashKeysRequest
                    {
                        Key = "product:*"
                    });
                    productKeys.UnionWith(allKeys.Keys);
                }
            }

            var products = new List<ProductDto?>();

            // Извлекаем данные из кэша и при необходимости из базы данных
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
    }
}
