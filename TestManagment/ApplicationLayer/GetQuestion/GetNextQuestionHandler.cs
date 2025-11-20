using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;
using TestManagment.Shared.Requests;

namespace TestManagment.ApplicationLayer.GetQuestion
{
    public class GetNextQuestionHandler : IRqtHandler<GetNextQuestionRequest, NextQuestion>
    {
        private readonly TestDbContext dbContext;

        public GetNextQuestionHandler(TestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<NextQuestion> Handle(GetNextQuestionRequest request)
        {
            if (request.QuestionIndex < 1)
            {
                throw new Exception("Question index should be greater than 0");
            }

            var nextQuestionId = await dbContext.TestsQuestions
                .Where(t => t.TestId == request.TestId)
                .OrderBy(q => q.QuestionId)
                .Skip(request.QuestionIndex - 1)
                .Take(1)
                .Select(t => t.QuestionId)
                .FirstOrDefaultAsync();

            if (nextQuestionId == 0)
            {
                throw new Exception("The Exam is end no more questions");
            }

            var NextQuestion = await dbContext.Questions
                .Where(q => q.Id == nextQuestionId)
                .Select(ObjectMapper.QuestionToNextQuestion(request.QuestionIndex))
                .FirstOrDefaultAsync();

            if (NextQuestion == null)
            {
                throw new Exception("The Question does not exist");
            }

            return NextQuestion;
        }
    }
}
