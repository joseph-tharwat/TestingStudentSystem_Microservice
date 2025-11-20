using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.Messaging;
using TestManagment.Domain.Events;

namespace TestManagment.ApplicationLayer.CreateQuestion
{
    public class ManyQuestionsCreatedEventHandler : IDomainEventHandler<ManyQuestionsCreatedEvent>
    {
        private readonly IEventPublisher eventPublisher;

        public ManyQuestionsCreatedEventHandler(IEventPublisher eventPublisher)
        {
            this.eventPublisher = eventPublisher;
        }

        public async Task Publish(ManyQuestionsCreatedEvent e)
        {
            await eventPublisher.PublishManyQuestionsCreatedAsync(e);
        }
    }
}
