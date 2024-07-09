namespace CoffeeShop.Products.Api.Models.Dto
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public List<string> Subcategories { get; set; }
    }
}
