using CoffeeShop.DataAccess.Common;
using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Storage
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync();
        Task<List<Product>> GetProductsAsync(Filter filter);
    }
}
