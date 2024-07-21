using AutoMapper;
using ProductGRPCService;
using Filter = CoffeeShop.Products.Api.Models.Filter;
using Product = CoffeeShop.Products.Api.Models.Product;
using Category = CoffeeShop.Products.Api.Models.Category;
using FilterGrpc = ProductGRPCService.Filter;
using CategoryGrpc = ProductGRPCService.Category;
using ProductGrpc = ProductGRPCService.Product;

namespace CoffeeShop.Products.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryGrpc>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories.Select(sc => sc.Name).ToList()));
            CreateMap<Product, ProductGrpc>();
            CreateMap<ProductGrpc, Product>();
            CreateMap<FilterRequest, Filter>();
            CreateMap<Filter, FilterGrpc>();
        }
    }
}
