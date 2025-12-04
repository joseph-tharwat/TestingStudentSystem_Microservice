using TestManagment.Shared.Result;

namespace TestManagment.Domain.DomainErrors
{
    public static class QuestionErrors
    {
        public static ErrorNote EmptyText=> new ErrorNote(ErrorType.DomainRuleViolation, "The question text must not be null or empty.");
        
    }
}
