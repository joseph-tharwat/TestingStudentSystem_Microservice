using System.Collections.ObjectModel;
using TestManagment.Domain.ValueObjects.Question;

namespace TestManagment.Domain.Entities
{
    public class Question
    {
        public int Id { get; private set; }
        public QuestionTxt QuestionText { get;private set; }
        public QuestionChoise Choise1 { get; private set; }
        public QuestionChoise Choise2 { get; private set; }
        public QuestionChoise Choise3 { get; private set; }
        public QuestionAnswer Answer {  get; private set; }
        public Collection<TestQuestion> TestQuestions { get; set; }

        public Question()
        {
            
        }
        public Question(QuestionTxt questionTxt, QuestionChoise choise1, QuestionChoise choise2, QuestionChoise choise3, QuestionAnswer answer)
        {
            this.QuestionText = questionTxt;
            this.Choise1 = choise1;
            this.Choise2 = choise2;
            this.Choise3 = choise3;
            this.Answer = answer;
        }
    }
}
