using CoffeeShop.Products.Api.Mapping;
using CoffeeShop.Products.Api.Repository;
using CoffeeShop.Products.Api.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), [typeof(Program).Assembly], ServiceLifetime.Singleton);
builder.Services.AddDbContext<DataContext>();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);

//builder.Host.ConfigureAutofac(Assembly.GetExecutingAssembly());

builder.Services.AddHttpClient();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});

var app = builder.Build();

//app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
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