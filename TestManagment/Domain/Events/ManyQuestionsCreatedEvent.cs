
namespace TestManagment.Domain.Events
{
    public record ManyQuestionsCreatedEvent(List<QuestionCreatedInfo> QuestionsCreatedInfo) :IDomainEvent;
}
