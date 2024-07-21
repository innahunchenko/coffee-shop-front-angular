using CoffeeShop.Products.Api.Mapping;
using CoffeeShop.Products.Api.Repository;
using CoffeeShop.Products.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), [typeof(Program).Assembly], ServiceLifetime.Singleton);
builder.Services.AddDbContext<DataContext>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
var app = builder.Build();
app.MapGrpcService<ProductGrpcService>();
ApplyMigration(app);
app.Run();

void ApplyMigration(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}