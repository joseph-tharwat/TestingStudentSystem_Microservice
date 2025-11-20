using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.ApplicationLayer.GetQuestion
{
    public class GetQuestionService
    {
        private readonly TestDbContext dbContext;

        public GetQuestionService(TestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<NextQuestion> GetNextQuestionAsync(StudentProgress studentProgress)
        {
            if(studentProgress.QuestionIndex < 1)
            {
                throw new Exception("Question index should be greater than 0");
            }
            
            var nextQuestionId = await dbContext.TestsQuestions
                .Where(t=>t.TestId == studentProgress.TestId)
                .OrderBy(q=>q.QuestionId)
                .Skip(studentProgress.QuestionIndex - 1)
                .Take(1)
                .Select(t=>t.QuestionId)
                .FirstOrDefaultAsync();

            if(nextQuestionId == 0)
            {
                throw new Exception("The Exam is end no more questions");
            }

            var NextQuestion = await dbContext.Questions
                .Where(q=> q.Id == nextQuestionId)
                .Select(ObjectMapper.QuestionToNextQuestion(studentProgress.QuestionIndex))
                .FirstOrDefaultAsync();

            if (NextQuestion == null)
            {
                throw new Exception("The Question does not exist");
            }

            return NextQuestion;

        }
    }
}
