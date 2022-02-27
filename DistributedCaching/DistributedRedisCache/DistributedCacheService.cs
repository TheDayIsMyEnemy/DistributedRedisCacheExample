using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedRedisCache
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            T? itemValue = default(T);
            var byteArr = await _distributedCache.GetAsync(key);
            if (byteArr != null)
                itemValue = await JsonSerializer.DeserializeAsync<T>(new MemoryStream(byteArr));

            return itemValue;
        }

        public async Task SetAsync<T>(string key, T itemValue)
        {
            var byteArr = JsonSerializer.SerializeToUtf8Bytes(itemValue);
            await _distributedCache.SetAsync(key, byteArr);
        }

        public async Task SetAsync<T>(string key, T itemValue, DistributedCacheEntryOptions options)
        {
            var byteArr = JsonSerializer.SerializeToUtf8Bytes(itemValue);
            await _distributedCache.SetAsync(key, byteArr, options);
        }
    }
}
