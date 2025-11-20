using System.ComponentModel.DataAnnotations;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;

namespace TestManagment.Shared.Dtos
{
    public class CreateTestCmd:ICmd
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "Test title can not be empty")]
        public string TestTitle { get; set; }

        [MinLength(1, ErrorMessage = "At least one question id should be provided")]
        public List<int> questionsIds {  get; set; }
    }
}
