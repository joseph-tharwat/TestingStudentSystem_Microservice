
using TestManagment.ApplicationLayer.Interfaces.TestReminder;

namespace TestManagment.Infrastructure.TestReminder
{
    public class TestNotificationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public TestNotificationWorker(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceScopeFactory.CreateScope();
                ITestReminderService? reminder = scope.ServiceProvider.GetService<ITestReminderService>();
                await reminder.SendUpcommingTestsNotificationsAsync();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
