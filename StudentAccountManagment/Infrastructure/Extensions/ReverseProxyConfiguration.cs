using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;

namespace StudentAccountManagment.Infrastructure.Extensions
{
    public static class ReverseProxyConfiguration
    {
        public static IServiceCollection ConfigureReverseProxy(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"))
                .AddTransforms(context =>
                {
                    if (context.Route.RouteId == "TestCreationGetRoute")
                    {
                        context.AddRequestTransform(transformContext =>
                        {
                            var username = transformContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                            transformContext.ProxyRequest.Headers.Add("x-UserName", username);
                            return ValueTask.CompletedTask;
                        });
                    }

                    if (context.Route.RouteId == "TestObservationRoute")
                    {
                        context.AddRequestTransform(transformContext =>
                        {
                            var access_token = transformContext.HttpContext.Request.Query["access_token"];
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var jwtToken = tokenHandler.ReadJwtToken(access_token);
                            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                            if (role != "Teacher")   //Block the request
                            {
                                transformContext.HttpContext.Response.StatusCode = 401;
                            }
                            //else if role = Teacher => Pass the request
                            return ValueTask.CompletedTask;
                        });
                    }

                    if (context.Route.RouteId == "GradeStudentRoute")
                    {
                        context.AddRequestTransform(transformContext =>
                        {
                            string studentId = transformContext.HttpContext.User.FindFirst("sid").Value;
                            transformContext.ProxyRequest.Headers.Remove("X-StudentId");
                            transformContext.ProxyRequest.Headers.Add("X-StudentId", studentId);
                            return ValueTask.CompletedTask;
                        });
                    }

                });

            return services;
        }
    }
}
