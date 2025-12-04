using Serilog;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.Logging
{
    public class LoggingCmdHandlerDecorator<TCmd> : ICmdHandler<TCmd>
        where TCmd : ICmd
    {
        private readonly ICmdHandler<TCmd> cmdHandler;

        public LoggingCmdHandlerDecorator(ICmdHandler<TCmd> innerHandler)
        {
            this.cmdHandler = innerHandler;
        }
        public async Task<Result> Handle(TCmd cmd)
        {
            Log.Logger.Information("Start handling the cmd " + typeof(TCmd).Name); 
            try
            {
                return await cmdHandler.Handle(cmd);
               
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error while handling " + typeof(TCmd).Name + ": " + ex.Message);
            }
            return null;
        }
    }
}
