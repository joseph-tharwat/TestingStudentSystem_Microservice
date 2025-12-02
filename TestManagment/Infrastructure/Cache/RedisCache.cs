using Microsoft.Extensions.Caching.Distributed;
using TestManagment.ApplicationLayer.Interfaces.Caching;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.Infrastructure.Cache
{
    //Not Implemented yet
    public class RedisCache : ICache
    {
        private readonly IDistributedCache cache;

        public RedisCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public IGResult GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetValue(string key, IGResult value)
        {
            throw new NotImplementedException();
        }
    }
}
