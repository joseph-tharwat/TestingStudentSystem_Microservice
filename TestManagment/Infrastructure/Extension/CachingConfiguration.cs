using TestManagment.ApplicationLayer.Interfaces.Caching;
using TestManagment.Infrastructure.Cache;

namespace TestManagment.Infrastructure.Extension
{
    public static class CachingConfiguration
    {
        public static IServiceCollection InjectInMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ICache, InMemoryCache>();
            return services;
        }
    }
}
