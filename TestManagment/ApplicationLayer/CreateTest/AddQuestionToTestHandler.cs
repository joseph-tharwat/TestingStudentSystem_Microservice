using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.ErrorsNotes;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.DomainErrors;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;
using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.CreateTest
{
    public class AddQuestionToTestHandler : ICmdHandler<AddQuestionToTestCmd>
    {
        private readonly TestDbContext dbContext;

        public AddQuestionToTestHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Result> Handle(AddQuestionToTestCmd cmd)
        {
            if (cmd.testId == 0)
            {
                return Result.Failure(TestErrors.NullTestId);
            }
            if (cmd.questionId == 0)
            {
                return Result.Failure(TestErrors.NullQuestionId);
            }

            var test = await dbContext.Tests.Where(t => t.Id == cmd.testId).Include(t => t.TestQuestions).FirstOrDefaultAsync();
            if (test == null)
            {
                return Result.Failure(TestErrors.InvalidTestId);
            }

            Result result = test.AddQuestion(cmd.questionId);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Failure(DatabaseErrors.FailedDuringSaveChanges);
            }
            return result;
        }
    }
}
