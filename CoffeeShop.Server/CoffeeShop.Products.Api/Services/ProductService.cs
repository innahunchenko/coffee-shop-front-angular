using AutoMapper;
using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;
using CoffeeShop.Products.Api.Repository;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await productRepository.GetMainCategoriesWithSubcategoriesAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<List<ProductDto>> GetProductsAsync(Filter filter)
        {
            var products = await productRepository.GetProductsAsync(filter);
            return mapper.Map<List<ProductDto>>(products);
        }
    }
}
