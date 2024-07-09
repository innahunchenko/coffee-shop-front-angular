using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<List<CategoryDto>> GetCategories();
        Task<List<ProductDto>> GetProductsAsync(Filter filter);
    }
}
