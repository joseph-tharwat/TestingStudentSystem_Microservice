namespace TestManagment.ApplicationLayer.Interfaces.CmdMediator
{
    public interface ICmdHandler<TCmd> 
        where TCmd : ICmd
    {
        public Task Handle(TCmd cmd);
    }
}
