using Microsoft.Extensions.Caching.Memory;
using TestManagment.ApplicationLayer.Interfaces.Caching;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.Infrastructure.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache cache;

        public InMemoryCache(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public Task SetValue(string key, IGResult value)
        {
            cache.Set(key, value);
            return Task.CompletedTask;
        }

        IGResult ICache.GetValue(string key)
        {
            cache.TryGetValue(key, out IGResult value);
            return value;
        }
    }
}
