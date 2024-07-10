using CoffeeShop.Products.Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Products.Api.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("ProductsConnection");
            
            try
            {
                var connection = new SqlConnection(connectionString);
                optionsBuilder.UseSqlServer(connection);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Server connection error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Incorrect operation: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Incorrect argument: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var categories = new[]
            {
                new Category { Id = 1, Name = "Coffee" },
                new Category { Id = 2, Name = "Accessories" },
                new Category { Id = 3, Name = "FoodItems" },
                new Category { Id = 4, Name = "Others" },

                new Category { Id = 5, Name = "WholeBean", ParentCategoryId = 1 },
                new Category { Id = 6, Name = "Ground", ParentCategoryId = 1 },
                new Category { Id = 7, Name = "Instant", ParentCategoryId = 1 },
                new Category { Id = 8, Name = "Pods", ParentCategoryId = 1 },
                new Category { Id = 9, Name = "Specialty", ParentCategoryId = 1 },

                new Category { Id = 10, Name = "CoffeeMakers", ParentCategoryId = 2 },
                new Category { Id = 11, Name = "Grinders", ParentCategoryId = 2 },
                new Category { Id = 12, Name = "Mugs", ParentCategoryId = 2 },
                new Category { Id = 13, Name = "Filters", ParentCategoryId = 2 },

                new Category { Id = 14, Name = "Pastries", ParentCategoryId = 3 },
                new Category { Id = 15, Name = "Snacks", ParentCategoryId = 3 },
                new Category { Id = 16, Name = "Syrups", ParentCategoryId = 3 },
                new Category { Id = 17, Name = "MilkAlternatives", ParentCategoryId = 3 },

                new Category { Id = 18, Name = "Merchandise", ParentCategoryId = 4 },
                new Category { Id = 19, Name = "Subscriptions", ParentCategoryId = 4 }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            var products = new List<Product>();
            var random = new Random();
            var subcategoryNames = new Dictionary<int, string>
            {
                { 5, "WholeBean" },
                { 6, "Ground" },
                { 7, "Instant" },
                { 8, "Pods" },
                { 9, "Specialty" },
                { 10, "CoffeeMakers" },
                { 11, "Grinders" },
                { 12, "Mugs" },
                { 13, "Filters" },
                { 14, "Pastries" },
                { 15, "Snacks" },
                { 16, "Syrups" },
                { 17, "MilkAlternatives" },
                { 18, "Merchandise" },
                { 19, "Subscriptions" }
            };

            // Add products to each subcategory with different counts between 15 and 30
            for (int i = 5; i <= 19; i++)
            {
                int productCount = random.Next(5, 11);
                for (int j = 0; j < productCount; j++)
                {
                    products.Add(new Product
                    {
                        Id = products.Count + 1,
                        Name = $"{subcategoryNames[i]}_Product_{products.Count + 1}",
                        Price = (decimal)(random.NextDouble() * 100),
                        CategoryId = i,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
            }

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
