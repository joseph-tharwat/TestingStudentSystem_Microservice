using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.Interfaces.CmdMediator
{
    public interface ICmdHandler<TCmd> 
        where TCmd : ICmd
    {
        public Task<Result> Handle(TCmd cmd);
    }
}
