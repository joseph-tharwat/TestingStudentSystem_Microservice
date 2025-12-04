using TestManagment.Shared.Result;

namespace TestManagment.Domain.DomainErrors
{
    public static class TestErrors
    {
        public static ErrorNote AlreadyPublishedCantAddQuestion => 
            new ErrorNote(ErrorType.DomainRuleViolation, "The test has been published, You can not add any questions now.");

        public static ErrorNote QuestionAddedBefore =>
            new ErrorNote(ErrorType.DomainRuleViolation, "The question is already added before");

        public static ErrorNote EmptyTestTitle =>
            new ErrorNote(ErrorType.DomainRuleViolation, "The title must not be null or empty.");


        public static ErrorNote InvalidQuestionIds(List<int> invalidIds) =>
            new ErrorNote(ErrorType.DomainRuleViolation, $"Invalid question ids {string.Join(",", invalidIds)}");

        public static ErrorNote NullTestId =>
            new ErrorNote(ErrorType.DomainRuleViolation,"Test id mus not be null or 0");

        public static ErrorNote NullQuestionId =>
            new ErrorNote(ErrorType.DomainRuleViolation, "Question id mus not be null or 0");

        public static ErrorNote InvalidTestId =>
            new ErrorNote(ErrorType.DomainRuleViolation, "Invalid test id");

    }
}
