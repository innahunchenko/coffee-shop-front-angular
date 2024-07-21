using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGRPCService;
using static ProductGRPCService.ProductService;

namespace CoffeeShop.Products.Api.Services
{
    public class ProductGrpcService : ProductServiceBase
    {
        private readonly IProductService productService;

        public ProductGrpcService(IProductService productService)
        {
            this.productService = productService;
        }

        public override async Task<CategoriesResponse> GetCategories(Empty request, ServerCallContext context)
        {
            var categories = await productService.GetCategoriesAsync();
            var response = new CategoriesResponse();
            response.Categories.AddRange(categories);
            return response;
        }

        public override async Task<ProductsResponse> GetProducts(FilterRequest request, ServerCallContext context)
        {
            var products = await productService.GetProductsAsync(request);
            var response = new ProductsResponse();
            response.Products.AddRange(products);
            return response;
        }
    }
}
