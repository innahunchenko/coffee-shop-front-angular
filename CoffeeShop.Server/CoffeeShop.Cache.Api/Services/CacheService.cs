using Grpc.Core;
using GrpcCache;
using CoffeeShop.Cache.Api.Repository;
using static GrpcCache.CacheService;
using StackExchange.Redis;
using KeyValuePair = GrpcCache.KeyValuePair;

public class CacheService : CacheServiceBase
{
    private readonly IRedisCacheRepository redisCacheRepository;

    public CacheService(IRedisCacheRepository redisCacheRepository)
    {
        this.redisCacheRepository = redisCacheRepository;
    }

    public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        var values = await redisCacheRepository.GetAllAsync(request.HashKey);

        var response = new GetAllResponse();
        foreach (var kvp in values)
        {
            response.Entries.Add(new KeyValuePair { Id = kvp.Key, Data = kvp.Value });
        }

        return response;
    }

    public override async Task<GetDataResponse> GetData(GetDataRequest request, ServerCallContext context)
    {
        var data = await redisCacheRepository.GetDataAsync(request.Key, request.Id);
        return new GetDataResponse { Data = data };
    }

    public override async Task<SetDataResponse> SetData(SetDataRequest request, ServerCallContext context)
    {
        await redisCacheRepository.SetDataAsync(request.Key, request.Id, request.Data);
        return new SetDataResponse { Success = true };
    }

    public override async Task<SetDataBatchResponse> SetDataBatch(SetDataBatchRequest request, ServerCallContext context)
    {
        var hashEntries = request.Entries.Select(entry => new HashEntry(entry.Id, entry.Data));
        await redisCacheRepository.SetDataAsync(request.Key, hashEntries);
        return new SetDataBatchResponse { Success = true };
    }

    public override async Task<GetHashKeysResponse> GetHashKeys(GetHashKeysRequest request, ServerCallContext context)
    {
        var keys = redisCacheRepository.GetHashKeys(request.Key);
        var response = new GetHashKeysResponse();
        response.Keys.AddRange(keys);
        return response;
    }

    public override async Task<SetAddResponse> SetAdd(SetAddRequest request, ServerCallContext context)
    {
        await redisCacheRepository.SetIndexAsync(request.IndexKey, request.ItemKey);
        return new SetAddResponse { Success = true };
    }

    public override async Task<GetIndexMembersResponse> GetIndexMembers(GetIndexMembersRequest request, ServerCallContext context)
    {
        var members = await redisCacheRepository.GetIndexMembers(request.IndexKey);
        var response = new GetIndexMembersResponse();
        response.Members.AddRange(members.Select(m => m.ToString()));
        return response;
    }

    public override async Task<RemoveDataResponse> RemoveData(RemoveDataRequest request, ServerCallContext context)
    {
        var success = await redisCacheRepository.RemoveDataAsync(request.Key, request.Id);
        return new RemoveDataResponse { Success = success };
    }

    public override async Task<RemoveHashResponse> RemoveHash(RemoveHashRequest request, ServerCallContext context)
    {
        var success = await redisCacheRepository.RemoveHashAsync(request.Key);
        return new RemoveHashResponse { Success = success };
    }
}
