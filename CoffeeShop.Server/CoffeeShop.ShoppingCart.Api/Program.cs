using CoffeeShop.ShoppingCart.Api.Repository;
using CoffeeShop.ShoppingCart.Api.Services;
using Grpc.Net.Client;
using GrpcProductsClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICartCosmosDbRepository, CartCosmosDbRepository>();
builder.Services.AddScoped<ICartService, CartService>();
//builder.Services.AddSingleton(sp =>
//{
//    var configuration = sp.GetRequiredService<IConfiguration>();
//    var cosmosDbConnectionString = configuration.GetConnectionString("CosmosDb");
//    return new CosmosClient(cosmosDbConnectionString);
//});

builder.Services.AddSingleton(sp =>
{
    var channel = GrpcChannel.ForAddress("http://products-api:8080");
    return new ProductsApi.ProductsApiClient(channel);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
