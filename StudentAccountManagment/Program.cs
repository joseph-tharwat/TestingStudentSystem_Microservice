using Serilog;
using SharedLogger;
using StudentAccountManagment.ApplicationLayer;
using StudentAccountManagment.ApplicationLayer.Extensions;
using StudentAccountManagment.Controllers;
using StudentAccountManagment.Infrastructure.Database;
using StudentAccountManagment.Infrastructure.Extensions;
using StudentAccountManagment.Infrastructure.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerUI();

SerilogSeqConfiguration.SerilogSeqConfigur("Auth", builder.Configuration);
builder.Host.UseSerilog();

builder.Services.InjectSqlDatabase(builder.Configuration);
builder.Services.InjectIdentity();

builder.Services.AddScoped<GetAllUsersEmailsHandler>();
builder.Services.AddGrpc();

builder.Services.InjectJwt(builder.Configuration);
builder.Services.AddJwt();

builder.Services.AddScoped<AuthService>();

builder.Services.AddRoleAuthorization();

builder.Services.ConfigureReverseProxy(builder.Configuration);

builder.Services.ConfigureCors();

builder.ConfigureKestrel();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.MigrateDb();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapReverseProxy();

app.UseCors("AllowAll");

app.MapControllers();

app.MapGrpcService<GetAllUserInfoEndPoints>();


app.Run();
