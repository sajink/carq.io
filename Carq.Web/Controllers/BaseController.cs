namespace Carq.Ops.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class BaseController : Controller
    {
        protected string _phone;
        protected bool _logged = false;
        protected IMemoryCache _cache;
        protected static JsonSerializerOptions camel = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        protected async Task<T> WithCache<T>(string key, int expirySeconds, Func<Task<T>> getItemDelegate) where T : class
        {
            T result;
            if (_cache.TryGetValue(key, out result)) 
                return result;
            result = await getItemDelegate();
            _cache.Set(key, result, DateTimeOffset.Now.AddSeconds(expirySeconds));
            return result;
        }
    }
}