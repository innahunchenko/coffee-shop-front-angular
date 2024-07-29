using CoffeeShop.Cache.Api.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<IRedisCacheRepository, RedisCacheRepository>();
var app = builder.Build();
app.MapGrpcService<CacheService>();
app.Run();
