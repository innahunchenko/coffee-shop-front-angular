using CoffeeShop.Products.Api.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace CoffeeShop.Products.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context)
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
            using (var connection = context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var query = GenerateQuery(filter, out var parameters);
                var products = await ExecuteQueryAsync(connection, query, parameters);

                var categoryIds = products.Select(p => p.CategoryId).Distinct().ToList();
                var categories = await context.Categories
                    .Include(c => c.ParentCategory)
                    .Where(c => categoryIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var product in products)
                {
                    product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }

                return products;
            }
        }

        private const string CommonQueryPart = @"
        SELECT p.*
        FROM Categories as c
        JOIN Categories as sbc ON sbc.ParentCategoryId = c.Id
        JOIN Products as p ON p.CategoryId = sbc.Id";

        private string GenerateQuery(Filter filter, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            var queryBuilder = new StringBuilder(CommonQueryPart);

            if (!string.IsNullOrEmpty(filter.Category))
            {
                queryBuilder.Append(" WHERE LOWER(c.Name) = LOWER(@CategoryName)");
                parameters.Add("@CategoryName", filter.Category);
            }
            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                queryBuilder.Append(" AND LOWER(sbc.Name) = LOWER(@SubcategoryName)");
                parameters.Add("@SubcategoryName", filter.Subcategory);
            }
            if (!string.IsNullOrEmpty(filter.Search))
            {
                queryBuilder.Append(" AND p.Name LIKE @Search");
                parameters.Add("@Search", $"%{filter.Search.Replace("_", "[_]")}%");
            }
            return queryBuilder.ToString();
        }

        private async Task<List<Product>> ExecuteQueryAsync(IDbConnection connection, string query, DynamicParameters parameters)
        {
            var products = await connection.QueryAsync<Product>(query, parameters);
            return products.ToList();
        }
    }
}
