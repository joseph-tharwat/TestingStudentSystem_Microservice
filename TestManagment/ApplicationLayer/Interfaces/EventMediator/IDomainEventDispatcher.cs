using TestManagment.Domain.Events;

namespace TestManagment.ApplicationLayer.Interfaces.EventMediator
{
    public interface IDomainEventDispatcher
    {
        public Task DispatchAsync(IDomainEvent e);
    }
}
