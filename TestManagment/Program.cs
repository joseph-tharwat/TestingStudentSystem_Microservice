using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLogger;
using TestManagment.ApplicationLayer.Extension;
using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using TestManagment.ApplicationLayer.TestNotifier;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Infrastructure.EventDispatcher;
using TestManagment.Infrastructure.Extension;
using TestManagment.Infrastructure.StudentsInfo;
using TestManagment.Infrastructure.TestReminder;
using TestManagment.PresentaionLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    }); 

builder.Services.AddOpenApi();

SerilogSeqConfiguration.SerilogSeqConfigur("TestManagment", builder.Configuration);
builder.Host.UseSerilog();

builder.Services.InjectSqlDatabase(builder.Configuration);

builder.Services.InjectRabbitMq(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.InjectHandlers();
builder.Services.DecorateHandlersWithLogging();

builder.Services.AddScoped<IDomainEventDispatcher, EventDispatcher>();

builder.Services.AddScoped<IGetAllStudentsService, GetAllstudentsGrpc>();

builder.Services.AddScoped<ITestReminderService, TestReminderByEmail>();

builder.Services.InjectGmailNotifer(builder.Configuration);

builder.Services.AddHostedService<TestNotificationWorker>();

builder.Services.AddGrpcClient<GetAllUsersInfo.GetAllUsersInfo.GetAllUsersInfoClient>(option => {
    option.Address = new Uri("https://localhost:5169");
});


builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerUI();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
    dbcontext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<TestObservationHub>("/TestObservation");

app.MapControllers();

app.Run();
