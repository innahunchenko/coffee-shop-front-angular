using CoffeeShop.Products.Api.Models;
using ProductGRPCService;
using CategoryGrpc = ProductGRPCService.Category;
using ProductGrpc = ProductGRPCService.Product;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<List<CategoryGrpc>> GetCategoriesAsync();
        Task<List<ProductGrpc>> GetProductsAsync(FilterRequest filter);
    }
}
