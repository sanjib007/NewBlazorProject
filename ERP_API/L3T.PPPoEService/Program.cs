using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.DataContext.PPPoEDBContext;
using L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;
using L3T.Infrastructure.Helpers.Implementation.PPPoE;
using L3T.Infrastructure.Helpers.Interface.PPPoE;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.PPPoEService.Extention;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContextPool<PPPoEDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PPPoE32Connection")); //I set this on appsettings.json
});

var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
builder.Services.AddDbContext<DMARadiusDBContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration.GetConnectionString("DMARadiusConnection"), serverVersion)
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);


builder.Services.AddScoped(typeof(ITestService), typeof(TestService));
builder.Services.AddScoped(typeof(IPPPoEService), typeof(PPPoEService));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

var configuration = builder.Configuration;

var url = configuration.GetValue<string>("Url:Auth");
var clientId = configuration.GetValue<string>("Url:ClientId");
var secret = configuration.GetValue<string>("Url:Secret");
builder.Services.AddOpeniddictExtention(url, clientId, secret);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
