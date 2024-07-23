using CoffeeShop.Cache.Api.Repository;
using CoffeeShop.Cache.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<IRedisCacheRepository, RedisCacheRepository>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
var app = builder.Build();
app.MapGrpcService<CacheGrpcService>();
app.Run();
