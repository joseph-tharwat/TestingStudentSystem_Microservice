using Serilog;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.ApplicationLayer.Logging
{
    public class LoggingRqtHandlerDecorator<TRqt, TResult> : IRqtHandler<TRqt, TResult>
        where TRqt : IRqt
        where TResult: IGResult
    {
        private readonly IRqtHandler<TRqt, TResult> rqtHandler;

        public LoggingRqtHandlerDecorator(IRqtHandler<TRqt, TResult> handler)
        {
            this.rqtHandler = handler;
        }

        public async Task<TResult> Handle(TRqt request)
        {
            Log.Logger.Information("Excution of the request " + typeof(TRqt).Name);

            return await rqtHandler.Handle(request);
        }
    }
}
