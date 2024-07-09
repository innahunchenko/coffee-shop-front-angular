using CoffeeShop.DataAccess.Common;
using CoffeeShop.Products.Api.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Products.Api.Storage
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly DataContext context;
        public ProductRepository(DataContext context) : base(context)
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

        public async Task<List<Product>> GetProductsAsync(Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                return await GetProductsBySearchAsync(filter.Search);
            }
            
            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                return await GetProductsBySubcategoryAsync(filter.Subcategory);
            }
            
            if (!string.IsNullOrEmpty(filter.Category))
            {
                return await GetProductsByCategoryAsync(filter.Category);
            }
            
            var query = "SELECT * FROM Products";
            return await ExecuteQueryAsync(query, new DynamicParameters());
        }

        private const string CommonQueryPart = @"
            SELECT p.*
            FROM Categories as c
            JOIN Categories as sbc ON sbc.ParentCategoryId = c.Id
            JOIN Products as p ON p.CategoryId = sbc.Id";

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            var query = CommonQueryPart + @"
                WHERE LOWER(c.Name) = LOWER(@CategoryName)";

            var parameters = new DynamicParameters();
            parameters.Add("@CategoryName", category);

            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<List<Product>> GetProductsBySubcategoryAsync(string subcategoryName)
        {
            var query = CommonQueryPart + @"
                WHERE LOWER(sbc.Name) = LOWER(@SubcategoryName)";

            var parameters = new DynamicParameters();
            parameters.Add("@SubcategoryName", subcategoryName);

            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<List<Product>> GetProductsBySearchAsync(string searchKeyword)
        {
            var query = CommonQueryPart + @"
                WHERE p.Name LIKE @Search";

            var parameters = new DynamicParameters();
            parameters.Add("@Search", $"%{searchKeyword.Replace("_", "[_]")}%");

            return await ExecuteQueryAsync(query, parameters);
        }

        private async Task<List<Product>> ExecuteQueryAsync(string query, DynamicParameters parameters)
        {
            using (var connection = context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                var products = await connection.QueryAsync<Product>(query, parameters);
                return products.ToList();
            }
        }
    }
}
