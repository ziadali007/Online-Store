
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using System.Text;

namespace Presentation
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService=context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var result= await cacheService.GetCacheValueAsync(cacheKey);

            if (!string.IsNullOrEmpty(result))
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var contextResult = await next.Invoke();

            if( contextResult.Result is OkObjectResult okResult)
            {
                await cacheService.SetCacheValueAsync(cacheKey, okResult.Value, TimeSpan.FromSeconds(durationInSec));
            }

        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();

            key.Append(request.Path);

            foreach(var item in request.Query.OrderBy(q=>q.Key))
            {
                key.Append($"| {item.Key}-{item.Value}");
            }

            return key.ToString();
        }
    }
}