namespace TestManagment.ApplicationLayer.Interfaces.TestReminder
{
    public interface IGetAllStudentsService
    {
        public Task<List<string>> GetAllEmails();
    }
}
