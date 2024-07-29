using AutoMapper;
using CoffeeShop.Products.Api.Models;
using CoffeeShop.Products.Api.Models.Dto;
using KeyValuePair = GrpcCacheClient.KeyValuePair;

namespace CoffeeShop.Products.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories.Select(sc => sc.Name).ToList()));
            CreateMap<CategoryDto, Category>();
            CreateMap<Product, ProductDto>()
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.ParentCategory != null ? src.Category.ParentCategory.Name : src.Category.Name))
               .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Category.ParentCategory == null ? string.Empty : src.Category.Name));
            CreateMap<ProductDto, Product>();
            CreateMap<ProductDto, IEnumerable<KeyValuePair>>()
                .ConvertUsing<ProductDtoToKeyValuePairConverter>();
            CreateMap<IEnumerable<KeyValuePair>, ProductDto>()
                .ConvertUsing<KeyValuePairToProductDtoConverter>();
        }
    }

    public class ProductDtoToKeyValuePairConverter : ITypeConverter<ProductDto, IEnumerable<KeyValuePair>>
    {
        public IEnumerable<KeyValuePair> Convert(ProductDto source, IEnumerable<KeyValuePair> destination, ResolutionContext context)
        {
            return source.GetType()
                .GetProperties()
                .Where(p => p.CanRead)
                .Select(p => new KeyValuePair
                {
                    Id = p.Name,
                    Data = p.GetValue(source)?.ToString() ?? string.Empty
                });
        }
    }

    public class KeyValuePairToProductDtoConverter : ITypeConverter<IEnumerable<KeyValuePair>, ProductDto>
    {
        public ProductDto Convert(IEnumerable<KeyValuePair> source, ProductDto destination, ResolutionContext context)
        {
            var entryDictionary = source.ToDictionary(e => e.Id, e => e.Data);
            var productDto = new ProductDto();

            foreach (var entry in entryDictionary)
            {
                var property = typeof(ProductDto).GetProperty(entry.Key);
                if (property != null && !string.IsNullOrEmpty(entry.Value))
                {
                    var value = System.Convert.ChangeType(entry.Value, property.PropertyType);
                    property.SetValue(productDto, value);
                }
            }

            return productDto;
        }
    }
}
