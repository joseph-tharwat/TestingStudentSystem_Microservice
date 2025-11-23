using TestManagment.Domain.Events;

namespace TestManagment.ApplicationLayer.Interfaces.EventMediator
{

    public interface IDomainEventHandler<TEvent> 
        where TEvent : IDomainEvent
    {
        public Task Publish(TEvent e); 
    }
}
