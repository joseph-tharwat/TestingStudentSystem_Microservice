
namespace TestManagment.Domain.Events
{
    public record OneQuestionCreatedEvent(QuestionCreatedInfo QuestionCreatedInfo) : IDomainEvent;
}
