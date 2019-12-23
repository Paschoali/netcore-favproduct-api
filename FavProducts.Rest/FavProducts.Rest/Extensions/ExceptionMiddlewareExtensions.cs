using FavProducts.Rest.Middleware;
using Microsoft.AspNetCore.Builder;

namespace FavProducts.Rest.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}