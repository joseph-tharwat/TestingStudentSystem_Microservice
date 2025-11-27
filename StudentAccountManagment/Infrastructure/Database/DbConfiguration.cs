using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StudentAccountManagment.Infrastructure.Database
{
    public static class DbConfiguration
    {
        public static IServiceCollection InjectSqlDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("local")));
            return services;    
        }

        public static IServiceCollection InjectIdentity(this IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static WebApplication MigrateDb(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                dbContext.Database.Migrate();

                 RoleSeeding.SeedRoles(scope.ServiceProvider).Wait();
            }

            return app;
        }
    }
}
