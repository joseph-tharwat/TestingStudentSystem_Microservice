using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.ApplicationLayer.CreateTest
{
    public class AddQuestionToTestHandler : ICmdHandler<AddQuestionToTestCmd>
    {
        private readonly TestDbContext dbContext;

        public AddQuestionToTestHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Handle(AddQuestionToTestCmd cmd)
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

            test.AddQuestion(cmd.questionId);
            await dbContext.SaveChangesAsync();
        }
    }
}
