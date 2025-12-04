using System.Linq.Expressions;
using TestManagment.Domain.Entities;
using TestManagment.Domain.Events;
using TestManagment.Domain.ValueObjects.Question;
using TestManagment.Domain.ValueObjects.Test;
using TestManagment.Shared.Requests;
using TestManagment.Shared.Result;

namespace TestManagment.Shared.Dtos
{
    public static partial class ObjectMapper
    {
        public static Expression<Func<CreateQuestionCmd, Question>> QuestionRequestToQuestion()
        {
            return request => new Question(
                    new QuestionTxt(request.QuestionText),
                    new QuestionChoise(request.Choise1),
                    new QuestionChoise(request.Choise2),
                    new QuestionChoise(request.Choise3),
                    new QuestionAnswer(request.Answer)
                    );
        }

        public static Question QuestionRequestToQuestion(CreateQuestionCmd request)
        {
            return new Question(
                    new QuestionTxt(request.QuestionText),
                    new QuestionChoise(request.Choise1),
                    new QuestionChoise(request.Choise2),
                    new QuestionChoise(request.Choise3),
                    new QuestionAnswer(request.Answer)
                    );
        }
        public static List<Question> QuestionRequestListToQuestionList(List<CreateQuestionCmd> requests)
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
        public static Result<Test> CreateTestRequestToTest(CreateTestCmd request)
        {
            Result<TestTitle> titleResult = TestTitle.Create(request.TestTitle);
            if(titleResult.IsFailure)
            {
                return Result<Test>.Failure(titleResult.Error);
            }

            var test = new Test(titleResult.Data);
            foreach (var id in request.questionsIds)
            {
                test.AddQuestion(id);
            }
            return Result<Test>.Success(test,null);
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
