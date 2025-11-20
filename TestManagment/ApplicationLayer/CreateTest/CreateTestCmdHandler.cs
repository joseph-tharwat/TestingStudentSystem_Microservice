using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.ApplicationLayer.CreateTest
{
    public class CreateTestCmdHandler : ICmdHandler<CreateTestCmd>
    {
        private readonly TestDbContext dbContext;

        public CreateTestCmdHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task Handle(CreateTestCmd cmd)
        {
            var validQuestionIds = await dbContext.Questions
                .Where(q => cmd.questionsIds.Contains(q.Id))
                .Select(q => q.Id)
                .ToListAsync();

            var invalidIds = cmd.questionsIds.Except(validQuestionIds).ToList();
            if (invalidIds.Count != 0)
            {
                throw new ArgumentException($"Invalid question ids {string.Join(",", invalidIds)}");
            }

            var test = ObjectMapper.CreateTestRequestToTest(cmd);
            await dbContext.AddAsync(test);
            await dbContext.SaveChangesAsync();
        }
    }
}
