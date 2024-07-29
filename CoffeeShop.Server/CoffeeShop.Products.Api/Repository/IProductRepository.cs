using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync();
        Task<List<Product>> GetProductsAsync(Filter filter);
    }
}
