using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLogger;
using TestManagment.ApplicationLayer.GetQuestion;
using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Infrastructure.RabbitMQ;
using TestManagment.PresentaionLayer;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;
using TestManagment.ApplicationLayer.Interfaces.Messaging;

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
