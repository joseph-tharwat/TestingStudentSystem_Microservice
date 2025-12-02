using System.Text.Json;
using TestManagment.ApplicationLayer.Interfaces.Caching;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.Infrastructure.Cache
{
    public class CacheDecorateor<TRequest, TGResult> : IRqtHandler<TRequest, TGResult>
        where TRequest : IRqt
        where TGResult : IGResult
    {
        private readonly IRqtHandler<TRequest, TGResult> rqtHandler;
        private readonly ICache cache;

        public CacheDecorateor(IRqtHandler<TRequest, TGResult> rqtHandler, ICache cache)
        {
            this.rqtHandler = rqtHandler;
            this.cache = cache;
        }

        public async Task<TGResult> Handle(TRequest request)
        {
            var cachedValue = cache.GetValue(request.ToString());
            if(cachedValue != null)
            {
                return (TGResult)cachedValue;
            }
            else
            {
                var result =  await rqtHandler.Handle(request);
                cache.SetValue(request.ToString(), result);
                return result;
            }
        }
    }
}
