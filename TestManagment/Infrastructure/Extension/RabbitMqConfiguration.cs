using Microsoft.EntityFrameworkCore;
using TestManagment.ApplicationLayer.Interfaces.Messaging;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Infrastructure.RabbitMQ;

namespace TestManagment.Infrastructure.Extension
{
    public static class RabbitMqConfiguration
    {
        public static IServiceCollection InjectRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSetings>(configuration.GetSection("RabbitMq"));
            services.AddSingleton<IEventPublisher, RabbitMqService>();

            return services;
        }
    }
}
