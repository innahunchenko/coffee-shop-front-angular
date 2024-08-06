using AutoMapper;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;
using GrpcCacheClient;
using Newtonsoft.Json;
using static GrpcCacheClient.CacheService;
using KeyValuePair = GrpcCacheClient.KeyValuePair;

namespace CoffeeShop.Products.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly CacheServiceClient cacheClient;

        private static readonly string CATEGORIES_KEY = "KEY:CATEGORIES";

        public CategoryService(IProductRepository productRepository,
                              IMapper mapper,
                              CacheServiceClient cacheClient)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.cacheClient = cacheClient;
        }

        public async Task<List<CategoryDto?>> GetCategoriesAsync()
        {
            var cachedCategories = await GetCachedCategoriesAsync();
            if (cachedCategories.Any())
            {
                Console.WriteLine($"categories from cache");
                return cachedCategories;
            }

            var categoriesFromDb = await GetCategoriesFromDbAsync();
            if (categoriesFromDb.Any())
            {
                await CacheCategoriesAsync(categoriesFromDb);
                Console.WriteLine($"categories from db");
                return categoriesFromDb;
            }

            return new List<CategoryDto?>();
        }

        private async Task<List<CategoryDto?>> GetCachedCategoriesAsync()
        {
            var cachedValues = await cacheClient.GetHashAllAsync(new GetHashAllRequest { HashKey = CATEGORIES_KEY });
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

            var request = new SetHashDataBatchRequest
            {
                Key = CATEGORIES_KEY,
                Entries = { hashEntries }
            };

            await cacheClient.SetHashDataBatchAsync(request);
        }
    }
}
