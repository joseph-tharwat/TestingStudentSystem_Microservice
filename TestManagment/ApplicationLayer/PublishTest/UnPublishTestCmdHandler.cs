using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.Entities;
using TestManagment.Infrastructure.DataBase;

namespace TestManagment.ApplicationLayer.PublishTest
{
    public class UnPublishTestCmdHandler : ICmdHandler<UnPublishTestCmd>
    {
        private readonly TestDbContext dbContext;

        public UnPublishTestCmdHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task Handle(UnPublishTestCmd cmd)
        {
            Test test = await dbContext.Tests.Where(t => t.Id == cmd.TestId).Include(t=>t.Schedulings).FirstOrDefaultAsync();
            if (test == null)
            {
                throw new ArgumentException("Test not exist");
            }
            test.UnPublish();
            await dbContext.SaveChangesAsync();
        }
    }
}
