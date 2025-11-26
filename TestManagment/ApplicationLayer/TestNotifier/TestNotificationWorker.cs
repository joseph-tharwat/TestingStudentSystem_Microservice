
using Serilog;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;

namespace TestManagment.ApplicationLayer.TestNotifier
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
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    ITestReminderService? reminder = scope.ServiceProvider.GetService<ITestReminderService>();
                    await reminder.SendUpcommingTestsNotificationsAsync();
                }
                catch (Exception ex)
                { 
                    Log.Logger.Error(ex.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
