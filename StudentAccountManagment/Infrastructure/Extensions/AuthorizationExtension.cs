namespace StudentAccountManagment.Infrastructure.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddRoleAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(conf =>
            {
                conf.AddPolicy("StudentPolicy", policy => policy.RequireRole(["Student"]));
                conf.AddPolicy("TeacherPolicy", policy => policy.RequireRole(["Teacher"]));
            });
            return services;
        }
    }
}
