using System.Reflection;
using CoffeeShop.Infrastructure;
using Grpc.Net.Client;
using ProductsClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.ConfigureAutofac(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton(sp =>
{
    var channel = GrpcChannel.ForAddress("http://products-api:8080");
    return new ProductService.ProductServiceClient(channel);
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
