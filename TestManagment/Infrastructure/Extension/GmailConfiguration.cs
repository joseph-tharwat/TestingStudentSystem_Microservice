using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using TestManagment.Infrastructure.Notifications;

namespace TestManagment.Infrastructure.Extension
{
    public static class GmailConfiguration
    {
        public static IServiceCollection InjectGmailNotifer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GmailSettings>(configuration.GetSection("Gmail"));
            services.AddScoped<INotifyService, GmailNotifer>();
            return services;
        }

    }
}
