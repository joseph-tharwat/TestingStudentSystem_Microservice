using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLogger;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.Messaging;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using TestManagment.ApplicationLayer.Logging;
using TestManagment.ApplicationLayer.TeastReminder;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Infrastructure.EventDispatcher;
using TestManagment.Infrastructure.RabbitMQ;
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

builder.Services.AddDbContext<TestDbContext>(ops=> ops.UseSqlServer(builder.Configuration.GetConnectionString("local")));
builder.Services.AddSingleton<IEventPublisher, RabbitMqService>();
builder.Services.Configure<RabbitMqSetings>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSignalR();

builder.Services.Scan(scan =>
    scan.FromAssemblies(typeof(ICmdHandler<>).Assembly,
                        typeof(IDomainEventHandler<>).Assembly)
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

builder.Services.Decorate(typeof(ICmdHandler<>), typeof(LoggingCmdHandlerDecorator<>));
builder.Services.Decorate(typeof(IRqtHandler<,>), typeof(LoggingRqtHandlerDecorator<,>));

builder.Services.AddScoped<IDomainEventDispatcher, EventDispatcher>();

builder.Services.AddScoped<IGetAllStudentsService, GetAllstudentsGrpc>();
builder.Services.AddScoped<ITestReminderService, TestReminderByEmail>();
builder.Services.AddScoped<INotifyService, EmailNotifer>();
builder.Services.AddHostedService<TestNotificationWorker>();
builder.Services.AddGrpcClient<GetAllUsersInfo.GetAllUsersInfo.GetAllUsersInfoClient>(option => {
    option.Address = new Uri("http://localhost:5169");
}
);

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
