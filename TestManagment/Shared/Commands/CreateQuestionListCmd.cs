using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.Shared.Dtos
{
    public class CreateQuestionListCmd: ICmd
    {
        public List<CreateQuestionCmd> List { get; set; }
    }
}
