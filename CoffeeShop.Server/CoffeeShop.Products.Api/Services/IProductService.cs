using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<List<CategoryDto?>> GetCategoriesAsync();
        Task<IEnumerable<ProductDto?>> GetProductsAsync(Filter filter);
    }
}
