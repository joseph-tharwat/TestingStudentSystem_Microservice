using System.ComponentModel.DataAnnotations;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;

namespace TestManagment.Shared.Requests
{
    public record NextQuestion(int QuestionIndex, int QuestionId, string QuestionText, string Choise1, string Choise2, string Choise3):IGResult;

}
