using CoffeeShop.Products.Api.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace CoffeeShop.Products.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext context;
        private IDbConnection connection;

        public ProductRepository(DataContext context)
        {
            this.context = context;
        }

        public IDbConnection GetConnection()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                connection = new SqlConnection(context.Database.GetConnectionString());
                connection.Open();
            }
            return connection;
        }

        public async Task<List<Category>> GetMainCategoriesWithSubcategoriesAsync()
        {
            return await context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.Subcategories)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductsAsync(Filter filter)
        {
            using (var connection = GetConnection())
            {
                var totalItemsQuery = GenerateCountQuery(filter, out var parameters);
                var totalItems = await connection.ExecuteScalarAsync<int>(totalItemsQuery, parameters);
                return totalItems;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(Filter filter, int pageNumber, int pageSize)
        {
            using (var connection = context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                var query = GenerateQuery(filter, out var parameters);

                // add pagination to query
                query += " ORDER BY p.Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                parameters.Add("@Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@PageSize", pageSize);

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

                return new List<Product>(products);
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

        private string GenerateCountQuery(Filter filter, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            var queryBuilder = new StringBuilder("SELECT COUNT(*) FROM (");
            queryBuilder.Append(CommonQueryPart);

            bool hasWhereClause = false;

            if (!string.IsNullOrEmpty(filter.Category))
            {
                queryBuilder.Append(" WHERE LOWER(c.Name) = LOWER(@CategoryName)");
                parameters.Add("@CategoryName", filter.Category);
                hasWhereClause = true;
            }
            if (!string.IsNullOrEmpty(filter.Subcategory))
            {
                queryBuilder.Append(hasWhereClause ? " AND" : " WHERE");
                queryBuilder.Append(" LOWER(sbc.Name) = LOWER(@SubcategoryName)");
                parameters.Add("@SubcategoryName", filter.Subcategory);
                hasWhereClause = true;
            }
            if (!string.IsNullOrEmpty(filter.Search))
            {
                queryBuilder.Append(hasWhereClause ? " AND" : " WHERE");
                queryBuilder.Append(" p.Name LIKE @Search");
                parameters.Add("@Search", $"%{filter.Search.Replace("_", "[_]")}%");
            }

            queryBuilder.Append(") AS TotalCount");

            return queryBuilder.ToString();
        }

        private async Task<List<Product>> ExecuteQueryAsync(IDbConnection connection, string query, DynamicParameters parameters)
        {
            var products = await connection.QueryAsync<Product>(query, parameters);
            return products.ToList();
        }
    }
}
