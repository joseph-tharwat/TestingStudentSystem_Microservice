using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using TestManagment.Domain.Entities;
using TestManagment.Infrastructure.DataBase;

namespace TestManagment.ApplicationLayer.TeastReminder
{
    public class TestReminderByEmail : ITestReminderService
    {
        private readonly TestDbContext dbContext;
        private readonly INotifyService notifyService;
        private readonly IGetAllStudentsService getAllStudentsService;

        public TestReminderByEmail(TestDbContext dbContext, INotifyService notifyService, IGetAllStudentsService getAllStudentsService)
        {
            this.dbContext = dbContext;
            this.notifyService = notifyService;
            this.getAllStudentsService = getAllStudentsService;
        }
        public async Task SendUpcommingTestsNotificationsAsync()
        {
            List<Test> testsToNotify = await dbContext.Tests
                .Include(t=>t.Schedulings)
                .Where(t=> t.IsNotified == false &&
                           t.Schedulings.DateTime.AddMinutes(1) < DateTime.UtcNow && //1 minutes for testing, it should be every 1 hour
                           t.Schedulings.DateTime.AddMinutes(2) > DateTime.UtcNow 
                            || true==true) // true for testing to neglict the time during the testing
                .ToListAsync();

            foreach (var test in testsToNotify)
            {
                test.Notify();
                List<string> studentEmails = await getAllStudentsService.GetAllEmails();
                await notifyService.Notify(studentEmails);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
