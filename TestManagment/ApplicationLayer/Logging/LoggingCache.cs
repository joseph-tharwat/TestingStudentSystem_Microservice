using Org.BouncyCastle.Asn1.Ocsp;
using Serilog;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.ApplicationLayer.Logging
{
    public class LoggingCache<TRequest, TGResult> : IRqtHandler<TRequest, TGResult>
        where TRequest : IRqt
        where TGResult : IGResult
    {
        private readonly IRqtHandler<TRequest, TGResult> cacheHandler;

        public LoggingCache(IRqtHandler<TRequest, TGResult> cacheHandler)
        {
            this.cacheHandler = cacheHandler;
        }
        public async Task<TGResult> Handle(TRequest request)
        {
            Log.Logger.Information("Try to get the value from Cache for " + typeof(TRequest).Name);
            return await cacheHandler.Handle(request);
        }
    }
}
