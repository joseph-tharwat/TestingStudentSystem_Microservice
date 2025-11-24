using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.ApplicationLayer.PublishTest
{
    public record PublishTestCmd(int TestId, DateTime ScheduleTime) :ICmd;
}
