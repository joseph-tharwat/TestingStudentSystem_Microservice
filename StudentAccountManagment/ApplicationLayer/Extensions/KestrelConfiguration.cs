using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace StudentAccountManagment.ApplicationLayer.Extensions
{
    public static class KestrelConfiguration
    {
        public static WebApplicationBuilder ConfigureKestrel(this WebApplicationBuilder builder)
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(5169, listenOption =>
                {
                    listenOption.UseHttps();
                    listenOption.Protocols = HttpProtocols.Http1AndHttp2;
                });
            });

            return builder;
        }

    }
}
