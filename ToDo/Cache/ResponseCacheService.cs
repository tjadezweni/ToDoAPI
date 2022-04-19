using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ToDo.Contracts.Cache;

namespace ToDo.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);
            var cacheEntryOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeToLive };

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, cacheEntryOptions);
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            return String.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
