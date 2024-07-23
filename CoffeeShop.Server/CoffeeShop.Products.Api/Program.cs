using CoffeeShop.Products.Api.Mapping;
using CoffeeShop.Products.Api.Repository;
using CoffeeShop.Products.Api.Services;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using GrpcCacheClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), [typeof(Program).Assembly], ServiceLifetime.Singleton);
builder.Services.AddDbContext<DataContext>();
builder.Services.AddSingleton(sp =>
{
    var channel = GrpcChannel.ForAddress("http://cache-api:8080");
    return new CacheApi.CacheApiClient(channel);
});
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