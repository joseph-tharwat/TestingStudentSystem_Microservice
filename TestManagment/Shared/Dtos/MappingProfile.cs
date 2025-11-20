using System.Linq.Expressions;
using TestManagment.Domain.Entities;
using TestManagment.Domain.Events;
using TestManagment.Domain.ValueObjects.Question;
using TestManagment.Domain.ValueObjects.Test;

namespace TestManagment.Shared.Dtos
{
    public static partial class ObjectMapper
    {
        public static Expression<Func<QuestionDto, Question>> QuestionRequestToQuestion()
        {
            return request => new Question(
                    new QuestionTxt(request.QuestionText),
                    new QuestionChoise(request.Choise1),
                    new QuestionChoise(request.Choise2),
                    new QuestionChoise(request.Choise3),
                    new QuestionAnswer(request.Answer)
                    );
        }

        public static Question QuestionRequestToQuestion(QuestionDto request)
        {
            return new Question(
                    new QuestionTxt(request.QuestionText),
                    new QuestionChoise(request.Choise1),
                    new QuestionChoise(request.Choise2),
                    new QuestionChoise(request.Choise3),
                    new QuestionAnswer(request.Answer)
                    );
        }
        public static List<Question> QuestionRequestListToQuestionList(List<QuestionDto> requests)
        {
            List<Question> questions = new List<Question>();
            foreach(var request in requests)
            {
                questions.Add(QuestionRequestToQuestion(request));
            }
            return questions;
        }

    }

    public static partial class ObjectMapper
    {
        public static Expression<Func<Question, QuestionCreatedInfo>> QuestionToQuestionCreatedInfo()
        {
            return question => new QuestionCreatedInfo(question.Id, question.Answer);
        }

        public static QuestionCreatedInfo QuestionToQuestionCreatedInfo(Question question)
        {
            return new QuestionCreatedInfo(question.Id, question.Answer);
        }

        public static List<QuestionCreatedInfo> QuestionListToQuestionCreatedInfoList(List<Question> questions)
        {
            List<QuestionCreatedInfo> questionCreatedInfoList = new List<QuestionCreatedInfo>();
            foreach(var question in questions)
            {
                questionCreatedInfoList.Add(QuestionToQuestionCreatedInfo(question));
            }
            return questionCreatedInfoList;
        }
    }

    public static partial class ObjectMapper
    {
        public static Test CreateTestRequestToTest(CreateTestDto request)
        {
            var test = new Test(new TestTitle(request.TestTitle));
            foreach (var id in request.questionsIds)
            {
                test.AddQuestion(id);
            }
            return test;
        }
    }

    public static partial class ObjectMapper
    {
        public static Expression<Func<Question, NextQuestion>> QuestionToNextQuestion(int questionIndex)
        {
            return question => new NextQuestion(
                questionIndex,
                question.Id,
                question.QuestionText.Value,
                question.Choise1.Value,
                question.Choise2.Value,
                question.Choise3.Value);
        }

        public static NextQuestion QuestionToNextQuestion(int questionIndex, Question question)
        {
            return new NextQuestion(
                questionIndex,
                question.Id, 
                question.QuestionText.Value,
                question.Choise1.Value, 
                question.Choise2.Value, 
                question.Choise3.Value);
        }
    }

}
