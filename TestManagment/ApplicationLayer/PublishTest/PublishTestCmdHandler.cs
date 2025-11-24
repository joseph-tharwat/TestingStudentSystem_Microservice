using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.Entities;
using TestManagment.Infrastructure.DataBase;

namespace TestManagment.ApplicationLayer.PublishTest
{
    public class PublishTestCmdHandler : ICmdHandler<PublishTestCmd>
    {
        private readonly TestDbContext dbContext;

        public PublishTestCmdHandler(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task Handle(PublishTestCmd cmd)
        {
            Test test = await dbContext.Tests.Where(t => t.Id == cmd.TestId).Include(t=>t.Schedulings).FirstOrDefaultAsync();
            if (test == null)
            {
                throw new ArgumentException("Test not exist");
            }
            test.Publish(cmd.ScheduleTime);
            await dbContext.SaveChangesAsync();
        }
    }
}
