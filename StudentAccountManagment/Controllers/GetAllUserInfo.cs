
using GetAllUsersInfo;
using Grpc.Core;
using StudentAccountManagment.ApplicationLayer;

namespace StudentAccountManagment.Controllers
{
    public class GetAllUserInfoEndPoints: GetAllUsersInfo.GetAllUsersInfo.GetAllUsersInfoBase
    {
        private readonly GetAllUsersEmailsHandler handler;

        public GetAllUserInfoEndPoints(GetAllUsersEmailsHandler handler)
        {
            this.handler = handler;
        }
        public override async Task<GetAllUsersEmailsResponse> GetAllUsersEmails(GetAllUsersEmailsRequest request, ServerCallContext context)
        {
            var emails = await handler.Handle();
            var result = new GetAllUsersEmailsResponse();
            result.Emails.AddRange(emails);
            return result;
        }
    }
}
