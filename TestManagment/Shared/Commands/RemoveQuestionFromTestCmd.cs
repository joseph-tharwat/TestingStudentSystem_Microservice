using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.Shared.Dtos
{
    public record RemoveQuestionFromTestCmd(int testId, int questionId):ICmd;
}
