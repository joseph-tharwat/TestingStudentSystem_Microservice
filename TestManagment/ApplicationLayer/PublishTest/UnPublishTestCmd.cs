using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.ApplicationLayer.PublishTest
{
    public record UnPublishTestCmd(int TestId) :ICmd;
}
