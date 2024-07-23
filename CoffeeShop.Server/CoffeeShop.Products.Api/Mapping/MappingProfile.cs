using AutoMapper;
using GrpcProducts;
using Filter = CoffeeShop.Products.Api.Models.Filter;
using Product = CoffeeShop.Products.Api.Models.Product;
using Category = CoffeeShop.Products.Api.Models.Category;
using FilterGrpc = GrpcProducts.Filter;
using CategoryGrpc = GrpcProducts.Category;
using ProductGrpc = GrpcProducts.Product;
using CoffeeShop.Products.Api.Models.Dto;

namespace CoffeeShop.Products.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryGrpc>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories.Select(sc => sc.Name).ToList()));
            CreateMap<CategoryGrpc, CategoryDto>();
            CreateMap<CategoryDto, CategoryGrpc>();
            CreateMap<Product, ProductGrpc>();
            CreateMap<ProductGrpc, Product>();
            CreateMap<FilterRequest, Filter>();
            CreateMap<Filter, FilterGrpc>();
        }
    }
}
