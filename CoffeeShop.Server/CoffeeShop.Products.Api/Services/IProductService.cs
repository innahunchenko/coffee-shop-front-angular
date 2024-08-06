using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Services
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto?>> GetProductsAsync(Filter filter, int pageNumber, int pageSize);
    }
}
