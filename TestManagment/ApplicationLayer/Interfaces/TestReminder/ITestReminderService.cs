namespace TestManagment.ApplicationLayer.Interfaces.TestReminder
{
    public interface ITestReminderService
    {
        public Task SendUpcommingTestsNotificationsAsync();
    }
}
