using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace TestManagment.Infrastructure.DataBase
{
    public static class DbConfiguration
    {
        public static IServiceCollection InjectSqlDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TestDbContext>(ops => ops.UseSqlServer(configuration.GetConnectionString("local")));
            return services;
        }
    }
}
