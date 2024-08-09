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
        private IDbConnection? connection;

        private const string CommonQueryPart = @"
            SELECT p.*
            FROM Categories as c
            JOIN Categories as sbc ON sbc.ParentCategoryId = c.Id
            JOIN Products as p ON p.CategoryId = sbc.Id";

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

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            var baseQuery = new StringBuilder(CommonQueryPart).ToString();
            var query = BuildQueryWithPagination(baseQuery, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            var baseQuery = new StringBuilder(CommonQueryPart)
                .Append(" WHERE LOWER(c.Name) = LOWER(@Category)")
                .ToString();
            parameters.Add("@Category", category);

            var query = BuildQueryWithPagination(baseQuery, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(string subcategory, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            var baseQuery = new StringBuilder(CommonQueryPart)
                .Append(" WHERE LOWER(sbc.Name) = LOWER(@Subcategory)")
                .ToString();
            parameters.Add("@Subcategory", subcategory);

            var query = BuildQueryWithPagination(baseQuery, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsByProductNameAsync(string productName, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            var baseQuery = new StringBuilder(CommonQueryPart)
                .Append(" WHERE p.Name LIKE @ProductName")
                .ToString();
            parameters.Add("@ProductName", $"%{productName.Replace("_", "[_]")}%");

            var query = BuildQueryWithPagination(baseQuery, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        private string BuildQueryWithPagination(string baseQuery, DynamicParameters parameters, int? pageNumber, int? pageSize)
        {
            var queryBuilder = new StringBuilder(baseQuery);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                queryBuilder.Append(" ORDER BY p.Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                parameters.Add("@Offset", (pageNumber.Value - 1) * pageSize.Value);
                parameters.Add("@PageSize", pageSize.Value);
            }

            return queryBuilder.ToString();
        }

        private async Task<IEnumerable<Product>> ExecuteProductQueryAsync(string query, DynamicParameters parameters)
        {
            using (var connection = GetConnection())
            {
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

        private async Task<List<Product>> ExecuteQueryAsync(IDbConnection connection, string query, DynamicParameters parameters)
        {
            var products = await connection.QueryAsync<Product>(query, parameters);
            return products.ToList();
        }

        public async Task<int> GetAllProductsTotalCountAsync()
        {
            var query = GenerateCountQuery();
            return await ExecuteCountQueryAsync(query, null);
        }

        public async Task<int> GetCategoryProductsTotalCountAsync(string category)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Category", category);
            var query = GenerateCountQuery("LOWER(c.Name) = LOWER(@Category)");
            return await ExecuteCountQueryAsync(query, parameters);
        }

        public async Task<int> GetSubcategoryProductsTotalCountAsync(string subcategory)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Subcategory", subcategory);
            var query = GenerateCountQuery("LOWER(sbc.Name) = LOWER(@Subcategory)");
            return await ExecuteCountQueryAsync(query, parameters);
        }

        public async Task<int> GetProductNameTotalCountAsync(string productName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", $"%{productName.Replace("_", "[_]")}%");
            var query = GenerateCountQuery("p.Name LIKE @ProductName");
            return await ExecuteCountQueryAsync(query, parameters);
        }

        private string GenerateCountQuery(string? filterCondition = null)
        {
            var queryBuilder = new StringBuilder("SELECT COUNT(*) FROM (");
            queryBuilder.Append(CommonQueryPart);

            if (!string.IsNullOrEmpty(filterCondition))
            {
                queryBuilder.Append(" WHERE ");
                queryBuilder.Append(filterCondition);
            }

            queryBuilder.Append(") AS TotalCount");
            return queryBuilder.ToString();
        }

        private async Task<int> ExecuteCountQueryAsync(string query, DynamicParameters? parameters)
        {
            using (var connection = GetConnection())
            {
                var totalItems = await connection.ExecuteScalarAsync<int>(query, parameters);
                return totalItems;
            }
        }
    }
}
