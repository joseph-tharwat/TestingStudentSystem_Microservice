using GetAllUsersInfo;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;

namespace TestManagment.Infrastructure.StudentsInfo
{
    public class GetAllstudentsGrpc:  IGetAllStudentsService
    {
        private readonly GetAllUsersInfo.GetAllUsersInfo.GetAllUsersInfoClient getAllUsersInfoClient;
        public GetAllstudentsGrpc(GetAllUsersInfo.GetAllUsersInfo.GetAllUsersInfoClient getAllUsersInfoClient)
        {
           this.getAllUsersInfoClient= getAllUsersInfoClient;
        }
        public async Task<List<string>> GetAllEmails()
        {
            GetAllUsersEmailsResponse studentsEmailsResponse = await getAllUsersInfoClient.GetAllUsersEmailsAsync(new GetAllUsersEmailsRequest());
            return studentsEmailsResponse.Emails.ToList();
        }
    }
}
