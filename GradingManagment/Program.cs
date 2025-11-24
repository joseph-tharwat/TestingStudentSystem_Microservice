using GradingManagment.ApplicationLayer;
using GradingManagment.Infrastructure.Database;
using GradingManagment.Infrastructure.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLogger;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerUI();

SerilogSeqConfiguration.SerilogSeqConfigur("Grading", builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddDbContext<GradingDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("local")));
builder.Services.Configure<RabbitMqSetings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddAutoMapper(conf => { }, typeof(Program));
builder.Services.AddHostedService<GetTestInfoWorker>();
builder.Services.AddScoped<GradeQuestionService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GradingDbContext>();
    dbContext.Database.Migrate();
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
