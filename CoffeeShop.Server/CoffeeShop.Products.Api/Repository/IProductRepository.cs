using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync();
        Task<IEnumerable<Product>> GetProductsAsync(Filter filter, int pageNumber, int pageSize);
        Task<int> GetTotalProductsAsync(Filter filter);
    }
}
