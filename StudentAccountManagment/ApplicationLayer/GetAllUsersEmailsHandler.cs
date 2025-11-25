using Microsoft.AspNetCore.Identity;
using StudentAccountManagment.Infrastructure;

namespace StudentAccountManagment.ApplicationLayer
{
    public class GetAllUsersEmailsHandler
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllUsersEmailsHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<string>> Handle()
        {
            IList<ApplicationUser> students = await userManager.GetUsersInRoleAsync("student");
            List<string> studentsEmails = students.Select(s => s.Email).ToList();
            return studentsEmails;
        }
    }
}
