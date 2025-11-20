using TestManagment.Domain.Events;

namespace TestManagment.ApplicationLayer.Interfaces.Messaging
{
    public interface IEventPublisher
    {
        public Task PublishOneQuestionCreatedAsync(OneQuestionCreatedEvent question);
        public Task PublishManyQuestionsCreatedAsync(ManyQuestionsCreatedEvent questions);

    }
}
