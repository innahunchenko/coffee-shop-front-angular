using AutoMapper;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using Newtonsoft.Json;
using GrpcProducts;
using System.Text;
using CategoryGrpc = GrpcProducts.Category;
using ProductGrpc = GrpcProducts.Product;
using Filter = CoffeeShop.Products.Api.Models.Filter;
using static GrpcCacheClient.CacheApi;
using GrpcCacheClient;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly CacheApiClient cacheApiClient;
        private readonly TimeSpan defaultExpiry;
     //   private readonly string cacheApiUrl;

        const string CATEGORY_KEYS = "categoryKeys";

        public ProductService(IProductRepository productRepository,
                              IConfiguration configuration,
                              IMapper mapper,
                              CacheApiClient cacheApiClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheApiClient = cacheApiClient;
            defaultExpiry = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:DefaultCacheDurationMinutes"));
       //     cacheApiUrl = configuration.GetValue<string>("CacheApiUrl");
        }

        public async Task<List<CategoryGrpc>> GetCategoriesAsync()
        {
            // Try to get cached categories
            var cachedCategories = await GetCachedCategoriesAsync();

            if (cachedCategories.Any())
            {
                return cachedCategories;
            }

            // If no cached categories, get categories from the database
            var categoriesFromDb = await GetCategoriesFromDbAsync();

            // Cache the fetched categories
            await CacheCategoriesAsync(categoriesFromDb);

            return categoriesFromDb;
        }

        private async Task<List<CategoryGrpc>> GetCachedCategoriesAsync()
        {
            var getAllCachedDataRequest = new GetAllCachedDataRequest { SetKey = CATEGORY_KEYS };
            var cachedData = await cacheApiClient.GetAllCachedDataAsync(getAllCachedDataRequest);

            Console.WriteLine($"Data to deserialize: {cachedData.Data}");
            var cachedCategories = new List<CategoryGrpc>();

            foreach (var data in cachedData.Data)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    try
                    {
                        var dtoCategory = JsonConvert.DeserializeObject<CategoryDto>(data);
                        var grpcCategory = mapper.Map<CategoryGrpc>(dtoCategory);
                        cachedCategories.Add(grpcCategory);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine($"Deserialization error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return cachedCategories;
        }

        private async Task<List<CategoryGrpc>> GetCategoriesFromDbAsync()
        {
            var categoriesFromDb = await productRepository.GetMainCategoriesWithSubcategoriesAsync();
            return mapper.Map<List<CategoryGrpc>>(categoriesFromDb);
        }

        private async Task CacheCategoriesAsync(List<CategoryGrpc> categories)
        {
            var tasks = new List<Task>();

            foreach (var category in categories)
            {
                var cacheKey = $"category:{category.Name}";
                var setCachedDataRequest = new SetCachedDataRequest
                {
                    CacheKey = cacheKey,
                    Data = JsonConvert.SerializeObject(category)
                };

                var addToSetRequest = new AddToSetRequest
                {
                    SetKey = CATEGORY_KEYS,
                    Value = cacheKey
                };

                tasks.Add(cacheApiClient.SetCachedDataAsync(setCachedDataRequest).ResponseAsync);
                tasks.Add(cacheApiClient.AddToSetAsync(addToSetRequest).ResponseAsync);
            }

            await Task.WhenAll(tasks);
        }

        public async Task<List<ProductGrpc>> GetProductsAsync(FilterRequest filterRequest)
        {
            var filter = mapper.Map<Filter>(filterRequest);
            var cacheKey = GenerateCacheKey(filter);
            var cachedProducts = await GetCachedDataAsync<List<ProductGrpc>>(cacheKey);
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = await productRepository.GetProductsAsync(filter);
            await SetCachedDataAsync(cacheKey, products, defaultExpiry);
            return mapper.Map<List<ProductGrpc>>(products);
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);
     //       await productRepository.AddProductAsync(product);
            await InvalidateCacheAsync("products:*");
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);
       //     await productRepository.UpdateProductAsync(product);
            await InvalidateCacheAsync("products:*");
        }

        public async Task DeleteProductAsync(int productId)
        {
         //   await productRepository.DeleteProductAsync(productId);
            await InvalidateCacheAsync("products:*");
        }

        private async Task<T> GetCachedDataAsync<T>(string cacheKey) where T : class
        {
            //var response = httpClient.GetAsync($"{cacheApiUrl}/{cacheKey}");
           // if (response.IsSuccessStatusCode)
           // {
             //   var content = await response.Content.ReadAsStringAsync();
             //   return JsonConvert.DeserializeObject<T>(content);
           // }

            return null;
        }

        private async Task SetCachedDataAsync<T>(string cacheKey, T data, TimeSpan expiry) where T : class
        {
          //  if (expiry == TimeSpan.Zero)
          //  {
          //      expiry = defaultExpiry;
          //  }

          //  await httpClient.PostAsJsonAsync($"{cacheApiUrl}/{cacheKey}/{expiry.TotalMinutes}", data);
        }

        private async Task InvalidateCacheAsync(string pattern)
        {
          //  await httpClient.DeleteAsync($"{cacheApiUrl}/{pattern}");
        }

        private string GenerateCacheKey(Filter filter)
        {
            var keyBuilder = new StringBuilder("products:");
            if (!string.IsNullOrEmpty(filter.Category))
            {
                keyBuilder.Append($"category:{filter.Category.ToLower()}:");
            }
            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                keyBuilder.Append($"subcategory:{filter.Subcategory.ToLower()}:");
            }
            if (!string.IsNullOrEmpty(filter.Search))
            {
                keyBuilder.Append($"search:{filter.Search.ToLower()}:");
            }
            return keyBuilder.ToString().TrimEnd(':');
        }
    }
}
