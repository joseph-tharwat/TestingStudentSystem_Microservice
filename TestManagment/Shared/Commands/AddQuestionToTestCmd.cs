using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.Shared.Dtos
{
    public record AddQuestionToTestCmd(int testId, int questionId):ICmd;
}
