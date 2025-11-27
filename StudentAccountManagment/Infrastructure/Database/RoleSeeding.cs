using Microsoft.AspNetCore.Identity;

namespace StudentAccountManagment.Infrastructure.Database
{
    public static class RoleSeeding
    {
        public async static Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Teacher", "Student" };

            foreach (var role in roles)
            {
                var existingRole = await roleManager.RoleExistsAsync(role);
                if(!existingRole)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
