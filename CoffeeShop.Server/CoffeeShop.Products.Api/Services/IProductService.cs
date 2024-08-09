using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto?>?> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<PaginatedList<ProductDto?>?> GetProductsBySubcategoryAsync(string subcategory, int pageNumber, int pageSize);
        Task<PaginatedList<ProductDto?>?> GetProductsByNameAsync(string name, int pageNumber, int pageSize);
        Task<PaginatedList<ProductDto?>?> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}
