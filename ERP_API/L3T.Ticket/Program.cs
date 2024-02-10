using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MediatR;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using MassTransit;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
//builder.Host.UseSerilog();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
// Add the memory cache services.
builder.Services.AddMemoryCache();

builder.Services.AddMediatR(typeof(Program));
//builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
 
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

builder.Services.AddDbContextPool<TicketDataWriteContext>(option =>
{ 
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<TicketDataReadContext>(option =>
{ 
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});





//builder.Services.AddDbContextPool<CamsDataWriteContext>(option =>
//{ 
//    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
//});
//builder.Services.AddDbContextPool<CamsDataReadContext>(option =>
//{ 
//    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
//});

// Add a custom scoped service.
//builder.Services.AddScoped<ITicketService, TicketService>();
//builder.Services.AddScoped<ICamsService, CamsService>();
builder.Services.AddTicketServiceDependency();

ConfigureMassTransit(builder.Services);

void ConfigureMassTransit(IServiceCollection services)
{
    builder.Services.AddMassTransit(c =>
    {
        c.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);

            cfg.Host(builder.Configuration["QueueSetting:HostName"], builder.Configuration["QueueSetting:VirtualHost"], h =>
            {
                h.Username(builder.Configuration["QueueSetting:UserName"]);
                h.Password(builder.Configuration["QueueSetting:Password"]);
            });

            cfg.ExchangeType = ExchangeType.Direct;
        });

    });

    builder.Services.AddMassTransitHostedService();
}


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization(); 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();


app.Run();
