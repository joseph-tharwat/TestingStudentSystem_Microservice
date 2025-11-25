namespace TestManagment.ApplicationLayer.Interfaces.TestReminder
{
    public interface INotifyService
    {
        public Task Notify(List<string> usersEmails);
    }
}
