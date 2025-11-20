using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.Shared.Requests
{
    public record GetNextQuestionRequest(int TestId, int QuestionIndex):IRqt;
}
