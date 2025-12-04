using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.ErrorsNotes;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.DomainErrors;
using TestManagment.Domain.Entities;
using TestManagment.Domain.SuccessNotes;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;
using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.CreateTest
{
    public class CreateTestCmdHandler : ICmdHandler<CreateTestCmd>
    {
        private readonly TestDbContext dbContext;

        public CreateTestCmdHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Result> Handle(CreateTestCmd cmd)
        {
            var validQuestionIds = await dbContext.Questions
                .Where(q => cmd.questionsIds.Contains(q.Id))
                .Select(q => q.Id)
                .ToListAsync();
            if(validQuestionIds.Count==0)
            {
                return Result.Failure(TestErrors.InvalidQuestionIds(cmd.questionsIds));
            }

            List<int> invalidIds = cmd.questionsIds.Except(validQuestionIds).ToList();
            if (invalidIds.Count != 0)
            {
                return Result.Failure(TestErrors.InvalidQuestionIds(invalidIds));
            }

            Result<Test> testResult = ObjectMapper.CreateTestRequestToTest(cmd);
            if (testResult.IsFailure)
            {
                return Result.Failure(testResult.Error);
            }

            try
            {
                await dbContext.AddAsync(testResult.Data);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Failure(DatabaseErrors.FailedDuringSaveChanges);
            }

            return Result.Success(TestNotes.CreatedSuccessfully);
        }
    }
}
