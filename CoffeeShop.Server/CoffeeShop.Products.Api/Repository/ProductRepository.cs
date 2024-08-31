﻿using CoffeeShop.Products.Api.Models;
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

        private const string BaseQuery = @"
            SELECT p.*
            FROM Categories AS c
            JOIN Categories AS sbc ON sbc.ParentCategoryId = c.Id
            JOIN Products AS p ON p.CategoryId = sbc.Id
            WHERE 1=1";

        public ProductRepository(DataContext context)
        {
            this.context = context;
        }

        private string BuildQueryWithCondition(string? condition)
        {
            var query = new StringBuilder(BaseQuery);

            if (!string.IsNullOrEmpty(condition))
            {
                query.Append(" AND ").Append(condition);
            }

            return query.ToString();
        }

        private string BuildQueryWithPagination(string baseQuery, DynamicParameters parameters, int pageNumber, int pageSize)
        {
            var queryBuilder = new StringBuilder(baseQuery);
            queryBuilder.Append(" ORDER BY p.Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
            parameters.Add("@Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@PageSize", pageSize);
            return queryBuilder.ToString();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            var baseQuery = new StringBuilder(BaseQuery).ToString();
            var query = BuildQueryWithPagination(baseQuery, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Category", category);
            var query = BuildQueryWithCondition("LOWER(c.Name) = LOWER(@Category)");
            query = BuildQueryWithPagination(query, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(string subcategory, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Subcategory", subcategory);
            var query = BuildQueryWithCondition("LOWER(sbc.Name) = LOWER(@Subcategory)");
            query = BuildQueryWithPagination(query, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(query, parameters);
        }

        public async Task<IEnumerable<Product>> GetProductsByProductNameAsync(string productName, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", $"%{productName.Replace("_", "[_]")}%");
            var condition = "p.Name LIKE @ProductName";
            var queryWithCondition = BuildQueryWithCondition(condition);
            var queryWithPagination = BuildQueryWithPagination(queryWithCondition, parameters, pageNumber, pageSize);
            return await ExecuteProductQueryAsync(queryWithPagination, parameters);
        }

        private async Task<IEnumerable<Product>> ExecuteProductQueryAsync(string query, DynamicParameters parameters)
        {
            using (var connection = new SqlConnection(context.Database.GetConnectionString()))
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

                return products;
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
            var queryWithCondition = BuildQueryWithCondition(filterCondition);
            return $"SELECT COUNT(*) FROM ({queryWithCondition}) AS TotalCountQuery";
        }

        private async Task<int> ExecuteCountQueryAsync(string query, DynamicParameters? parameters)
        {
            using (var connection = new SqlConnection(context.Database.GetConnectionString()))
            {
                return await connection.ExecuteScalarAsync<int>(query, parameters);
            }
        }
    }
}
