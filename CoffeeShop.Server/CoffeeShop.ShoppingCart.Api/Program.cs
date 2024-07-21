using System.Reflection;
using CoffeeShop.Infrastructure;
using Google.Protobuf.WellKnownTypes;
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

//builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(options =>
//{
//    options.Address = new Uri("http://products-api:8080");
//});

//var channel = GrpcChannel.ForAddress("http://products-api:8080");
//var client = new ProductService.ProductServiceClient(channel);
//var emptyRequest = new Empty();
//var categories = await client.GetCategoriesAsync(emptyRequest);

//foreach (var item in categories.Categories)
//{
//    Console.WriteLine(item.Name);
//}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
