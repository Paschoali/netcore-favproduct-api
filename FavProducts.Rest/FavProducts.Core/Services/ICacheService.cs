using System;
using System.Threading.Tasks;

namespace FavProducts.Core.Services
{
    public interface ICacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
        Task RemoveCachedAsync(string cacheKey);
    }
}