using Grpc.Core;
using GrpcCache;
using Newtonsoft.Json;
using static GrpcCache.CacheApi;

namespace CoffeeShop.Cache.Api.Services
{
    public class CacheGrpcService : CacheApiBase
    {
        private readonly IRedisCacheService redisCacheService;
        public CacheGrpcService(IRedisCacheService redisCacheService)
        {
            this.redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
        }

        public override async Task<GetCachedDataResponse> GetCachedData(GetCachedDataRequest request, ServerCallContext context)
        {
            var data = await redisCacheService.GetCachedDataAsync(request.CacheKey);
            return new GetCachedDataResponse { Data = data ?? string.Empty };
        }

        public override async Task<SetCachedDataResponse> SetCachedData(SetCachedDataRequest request, ServerCallContext context)
        {
            await redisCacheService.SetCachedDataAsync(request.CacheKey, request.Data);
            return new SetCachedDataResponse { Success = true };
        }

        public override async Task<SetCachedDataResponse> SetCachedDataWithExpiry(SetCachedDataWithExpiryRequest request, ServerCallContext context)
        {
            var expiry = TimeSpan.FromSeconds(request.ExpirySeconds);
            await redisCacheService.SetCachedDataAsync(request.CacheKey, request.Data, expiry);
            return new SetCachedDataResponse { Success = true };
        }

        public override async Task<InvalidateCacheResponse> InvalidateCache(InvalidateCacheRequest request, ServerCallContext context)
        {
            await redisCacheService.InvalidateCacheAsync(request.Pattern);
            return new InvalidateCacheResponse { Success = true };
        }

        public override async Task<AddToSetResponse> AddToSet(AddToSetRequest request, ServerCallContext context)
        {
            await redisCacheService.AddToSetAsync(request.SetKey, request.Value);
            return new AddToSetResponse { Success = true };
        }

        public override async Task<GetAllKeysResponse> GetAllKeys(GetAllKeysRequest request, ServerCallContext context)
        {
            var keys = await redisCacheService.GetAllKeysAsync(request.SetKey);
            var response = new GetAllKeysResponse();
            response.Keys.AddRange(keys);
            return response;
        }

        public override async Task<GetAllCachedDataResponse> GetAllCachedData(GetAllCachedDataRequest request, ServerCallContext context)
        {
            var cachedData = await redisCacheService.GetAllCachedDataAsync(request.SetKey);
            var response = new GetAllCachedDataResponse();
            response.Data.AddRange(cachedData);
            return response;
            //response.JsonData.AddRange(cachedData.Select(data => JsonConvert.SerializeObject(data)));
        }
    }
}
