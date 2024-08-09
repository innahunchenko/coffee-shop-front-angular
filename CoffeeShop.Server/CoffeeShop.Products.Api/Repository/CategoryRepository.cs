using CoffeeShop.Products.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoffeeShop.Products.Api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext context;

        public CategoryRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync()
        {
            return await context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.Subcategories)
                .ToListAsync();
        }
    }
}
