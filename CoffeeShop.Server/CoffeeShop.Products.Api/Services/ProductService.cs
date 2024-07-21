using AutoMapper;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using Newtonsoft.Json;
using ProductGRPCService;
using System.Text;
using CategoryGrpc = ProductGRPCService.Category;
using ProductGrpc = ProductGRPCService.Product;
using Filter = CoffeeShop.Products.Api.Models.Filter;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;
        private readonly TimeSpan defaultExpiry;
        private readonly string cacheApiUrl;

        public ProductService(IProductRepository productRepository,
                              IConfiguration configuration,
                              IMapper mapper,
                              HttpClient httpClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.httpClient = httpClient;
            defaultExpiry = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:DefaultCacheDurationMinutes"));
            cacheApiUrl = configuration.GetValue<string>("CacheApiUrl");
        }

        public async Task<List<CategoryGrpc>> GetCategoriesAsync()
        {
            //var cacheKey = "categories";
            //var cachedCategories = await GetCachedDataAsync<List<CategoryGrpc>>(cacheKey);
            //if (cachedCategories != null)
            //{
            //    return cachedCategories;
            //}

            var categories = await productRepository.GetMainCategoriesWithSubcategoriesAsync();
            //  await SetCachedDataAsync(cacheKey, categories, defaultExpiry);
            return mapper.Map<List<CategoryGrpc>>(categories);
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
            var response = await httpClient.GetAsync($"{cacheApiUrl}/{cacheKey}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        private async Task SetCachedDataAsync<T>(string cacheKey, T data, TimeSpan expiry) where T : class
        {
            if (expiry == TimeSpan.Zero)
            {
                expiry = defaultExpiry;
            }

            await httpClient.PostAsJsonAsync($"{cacheApiUrl}/{cacheKey}/{expiry.TotalMinutes}", data);
        }

        private async Task InvalidateCacheAsync(string pattern)
        {
            await httpClient.DeleteAsync($"{cacheApiUrl}/{pattern}");
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
