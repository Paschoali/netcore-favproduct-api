using FavProducts.Core.Rest.Transport;
using FavProducts.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace FavProducts.Rest.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService) => _cacheService = cacheService;

        [HttpDelete]
        public async Task Post(CacheDeleteRequest request)
        {
            await _cacheService.RemoveCachedAsync(request.Key);
        }
    }
}