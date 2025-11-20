using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.Messaging;
using TestManagment.Domain.Events;


namespace TestManagment.ApplicationLayer.CreateQuestion
{
    public class OneQuestionCreatedEventHandler : IDomainEventHandler<OneQuestionCreatedEvent>
    {
        private readonly IEventPublisher eventPublisher;
        public OneQuestionCreatedEventHandler(IEventPublisher eventPublisher)
        {
            this.eventPublisher = eventPublisher;
        }

        public async Task Publish(OneQuestionCreatedEvent e)
        {
            await eventPublisher.PublishOneQuestionCreatedAsync(e);
        }
    }
}
