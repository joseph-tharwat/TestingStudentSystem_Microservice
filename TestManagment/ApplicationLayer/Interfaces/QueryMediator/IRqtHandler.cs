namespace TestManagment.ApplicationLayer.Interfaces.QueryMediator
{
    public interface IRqtHandler<TRequest,TGResult>
        where TRequest : IRqt 
        where TGResult : IGResult
    {
        public Task<TGResult> Handle(TRequest request);
    }
}
