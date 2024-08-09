using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync();
    }
}
