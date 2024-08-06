using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto?>> GetCategoriesAsync();
    }
}
