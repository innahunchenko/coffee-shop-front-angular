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

    public override async Task<GetHashAllResponse> GetHashAll(GetHashAllRequest request, ServerCallContext context)
    {
        var values = await redisCacheRepository.GetHashAllAsync(request.HashKey);

        var response = new GetHashAllResponse();
        foreach (var kvp in values)
        {
            response.Entries.Add(new KeyValuePair { Id = kvp.Key, Data = kvp.Value });
        }

        return response;
    }

    public override async Task<GetHashDataResponse> GetHashData(GetHashDataRequest request, ServerCallContext context)
    {
        var data = await redisCacheRepository.GetHashDataAsync(request.Key, request.Id);
        return new GetHashDataResponse { Data = data };
    }

    public override async Task<SetHashDataResponse> SetHashData(SetHashDataRequest request, ServerCallContext context)
    {
        await redisCacheRepository.HashSetDataAsync(request.Key, request.Id, request.Data);
        return new SetHashDataResponse { Success = true };
    }

    public override async Task<SetHashDataBatchResponse> SetHashDataBatch(SetHashDataBatchRequest request, ServerCallContext context)
    {
        var hashEntries = request.Entries.Select(entry => new HashEntry(entry.Id, entry.Data));
        await redisCacheRepository.HashSetDataAsync(request.Key, hashEntries);
        return new SetHashDataBatchResponse { Success = true };
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

    public override async Task<RemoveHashDataResponse> RemoveHashData(RemoveHashDataRequest request, ServerCallContext context)
    {
        var success = await redisCacheRepository.RemoveHashDataAsync(request.Key, request.Id);
        return new RemoveHashDataResponse { Success = success };
    }

    public override async Task<RemoveHashKeyResponse> RemoveHashKey(RemoveHashKeyRequest request, ServerCallContext context)
    {
        var success = await redisCacheRepository.RemoveHashKeyAsync(request.Key);
        return new RemoveHashKeyResponse { Success = success };
    }

    public override async Task<SetValueResponse> SetValue(SetValueRequest request, ServerCallContext context)
    {
        await redisCacheRepository.SetValueAsync(request.Key, request.Value);

        return new SetValueResponse
        {
            Success = true
        };
    }

    public override async Task<GetValueResponse> GetValue(GetValueRequest request, ServerCallContext context)
    {
        try
        {

            var value = await redisCacheRepository.GetValueAsync(request.Key);
            return new GetValueResponse
            {
                Value = string.IsNullOrEmpty(value) ? "0" : value 
            };
        }
        catch (Exception ex) 
        { Console.WriteLine(ex); }

        return null;
    }
}
