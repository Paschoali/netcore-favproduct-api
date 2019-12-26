using FavProducts.Core.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;

        public CacheService(IDistributedCache cache, ILogger logger)
        {
            _cache = cache;
            _logger = logger.ForContext<CacheService>();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
                return;

            _logger.Debug($"Setting cache key: {cacheKey}.");

            var serializedResponse = JsonConvert.SerializeObject(response);

            await _cache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            _logger.Debug($"Getting cache key: {cacheKey}.");

            var cachedResponse = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedResponse))
            {
                _logger.Debug($"Cache key {cacheKey} does not exists or it is expired.");
                return null;
            }

            _logger.Debug($"Cache key {cacheKey} found. Returning cached response.");

            return cachedResponse;
        }

        public async Task RemoveCachedAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);

            _logger.Debug($"Cache key: {cacheKey} removed.");
        }
    }
}