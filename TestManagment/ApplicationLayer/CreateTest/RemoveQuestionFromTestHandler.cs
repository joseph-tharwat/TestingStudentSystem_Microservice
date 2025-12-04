using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;
using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.CreateTest
{
    public class RemoveQuestionFromTestHandler : ICmdHandler<RemoveQuestionFromTestCmd>
    {
        private readonly TestDbContext dbContext;

        public RemoveQuestionFromTestHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Result> Handle(RemoveQuestionFromTestCmd cmd)
        {
            if (cmd.testId == 0)
            {
                throw new ArgumentException("Invalid test id");
            }
            if (cmd.questionId == 0)
            {
                throw new ArgumentException("Invalid question id");
            }

            var test = await dbContext.Tests.Where(t => t.Id == cmd.testId).Include(t => t.TestQuestions).FirstOrDefaultAsync();
            if (test == null)
            {
                throw new ArgumentException("This test is not created");
            }

            test.RemoveQuestion(cmd.questionId);
            await dbContext.SaveChangesAsync();
            return null;
        }
    }
}
