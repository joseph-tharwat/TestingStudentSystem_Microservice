using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedLogger;
using System.Reflection;
using TestManagment.ApplicationLayer.CreateQuestion.Interfaces;
using TestManagment.ApplicationLayer.GetQuestion;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Infrastructure.RabbitMQ;
using TestManagment.PresentaionLayer;
using TestManagment.Services.CreateTest;
using TestManagment.Shared.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    }); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

SerilogSeqConfiguration.SerilogSeqConfigur("TestManagment", builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddDbContext<TestDbContext>(ops=> ops.UseSqlServer(builder.Configuration.GetConnectionString("local")));
builder.Services.AddScoped<CreateQuestionService>();
builder.Services.AddScoped<CreateTestService>();
builder.Services.AddSingleton<IEventPublisher, RabbitMqService>();
builder.Services.Configure<RabbitMqSetings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddMediatR(cnf=>cnf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<GetQuestionService>();

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerUI();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
    dbcontext.Database.Migrate();
}

    // Configure the HTTP request pipeline.
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
