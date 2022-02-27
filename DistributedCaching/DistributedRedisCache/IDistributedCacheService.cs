using Microsoft.Extensions.Caching.Distributed;

namespace DistributedRedisCache
{
    public interface IDistributedCacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T itemValue);

        Task SetAsync<T>(string key, T itemValue, DistributedCacheEntryOptions options);
    }
}
