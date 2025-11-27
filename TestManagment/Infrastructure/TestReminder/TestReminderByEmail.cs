using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using TestManagment.Domain.Entities;
using TestManagment.Infrastructure.DataBase;

namespace TestManagment.Infrastructure.TestReminder
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
                           t.Schedulings.DateTime.AddHours(1) <= DateTime.UtcNow && 
                           t.Schedulings.DateTime.AddHours(2) > DateTime.UtcNow || true==true)
                .ToListAsync();
            if (testsToNotify.Count == 0)
            {
                return;
            }

            List<string> studentEmails = await getAllStudentsService.GetAllEmails();
            if (studentEmails.Count == 0) 
            {
                return;
            }
            foreach (var test in testsToNotify)
            {
                test.Notify();
                await notifyService.Notify(studentEmails);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
