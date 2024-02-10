using L3T.Infrastructure.Helpers.Implementation.Ticket;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddScoped<ITicketService, TicketService>();

//ConfigureMassTransit(builder.Services);

/*void ConfigureMassTransit(IServiceCollection services)
{
    builder.Services.AddMassTransit(c =>
    {
        c.AddConsumer<TicketEntryConsumer>();
        c.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);

            cfg.Host(builder.Configuration["QueueSetting:HostName"], builder.Configuration["QueueSetting:VirtualHost"], h => {
                h.Username(builder.Configuration["QueueSetting:UserName"]);
                h.Password(builder.Configuration["QueueSetting:Password"]);
            });

            cfg.ExchangeType = ExchangeType.Direct;
        });

    });

    builder.Services.AddMassTransitHostedService();
}*/

/*builder.Services.AddDbContext<TicketDataWriteContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:WriteDB"], sqlserverOptions =>
    {
        sqlserverOptions.CommandTimeout(180); // 3 minutes
        sqlserverOptions.EnableRetryOnFailure(3);
        sqlserverOptions.MigrationsAssembly("L3T.Ticket.Consumers");
    });
});*/

/*builder.Services.AddDbContext<TicketDataReadContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ReadDB"], sqlserverOptions =>
    {
        sqlserverOptions.CommandTimeout(180); // 3 minutes
        sqlserverOptions.EnableRetryOnFailure(3);
        sqlserverOptions.MigrationsAssembly("L3T.Ticket.Consumers");
    });
});*/

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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapControllers();

app.Run();
