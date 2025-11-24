using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLogger;
using StudentAccountManagment.ApplicationLayer;
using StudentAccountManagment.Infrastructure;
using StudentAccountManagment.Infrastructure.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerUI();

SerilogSeqConfiguration.SerilogSeqConfigur("Auth", builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("local")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddJwt();
builder.Services.AddAuthorization(conf =>
{ 
    conf.AddPolicy("StudentPolicy", policy => policy.RequireRole(["Student"]));
    conf.AddPolicy("TeacherPolicy", policy => policy.RequireRole(["Teacher"]));
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();

    await RoleSeeding.SeedRoles(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapReverseProxy();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
