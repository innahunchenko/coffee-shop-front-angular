using GrpcProducts;
using CategoryGrpc = GrpcProducts.Category;
using ProductGrpc = GrpcProducts.Product;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<List<CategoryGrpc>> GetCategoriesAsync();
        Task<List<ProductGrpc>> GetProductsAsync(FilterRequest filter);
    }
}
