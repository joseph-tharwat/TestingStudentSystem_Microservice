using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;
using TestManagment.ApplicationLayer.Logging;
using TestManagment.Infrastructure.Cache;

namespace TestManagment.ApplicationLayer.Extension
{
    public static class HandlersConfiguration
    {
        public static IServiceCollection InjectHandlers(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblies(typeof(ICmdHandler<>).Assembly,
                                    typeof(IDomainEventHandler<>).Assembly,
                                    typeof(IRqtHandler<,>).Assembly)

                .AddClasses(classes => classes.AssignableTo(typeof(ICmdHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()

                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()

                .AddClasses(classes => classes.AssignableTo(typeof(IRqtHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }
        public static IServiceCollection DecorateHandlersWithLogging(this IServiceCollection services)
        {
            services.Decorate(typeof(ICmdHandler<>), typeof(LoggingCmdHandlerDecorator<>));
            services.Decorate(typeof(IRqtHandler<,>), typeof(LoggingRqtHandlerDecorator<,>));
            return services;
        }
        public static IServiceCollection DecorateHandlersWithCaching(this IServiceCollection services)
        {
            services.Decorate(typeof(IRqtHandler<,>), typeof(CacheDecorateor<,>));
            services.Decorate(typeof(IRqtHandler<,>), typeof(LoggingCache<,>));

            return services;
        }
    }
}
