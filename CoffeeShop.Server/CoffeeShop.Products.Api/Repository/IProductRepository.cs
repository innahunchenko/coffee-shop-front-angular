using CoffeeShop.Products.Api.Models;

namespace CoffeeShop.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(string subcategory, int pageNumber, int pageSize);
        Task<IEnumerable<Product>> GetProductsByProductNameAsync(string productName, int pageNumber, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<int> GetAllProductsTotalCountAsync();
        Task<int> GetCategoryProductsTotalCountAsync(string category);
        Task<int> GetSubcategoryProductsTotalCountAsync(string subcategory);
        Task<int> GetProductNameTotalCountAsync(string productName);
    }
}
