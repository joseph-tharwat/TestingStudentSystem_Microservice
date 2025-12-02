using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.ApplicationLayer.Interfaces.Caching
{
    public interface ICache
    {
        public IGResult GetValue(string key);
        public Task SetValue(string key, IGResult value);
    }
}
