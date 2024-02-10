using Autofac.Core;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.IPWhiteListDBContext;
using L3T.Infrastructure.Helpers.Models.Mikrotik.SocketModel;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1.5.5", Title = "My API" });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
// var url = configuration.GetValue<string>("Url:Auth");
// var clientId = configuration.GetValue<string>("Url:ClientId");
// var secret = configuration.GetValue<string>("Url:Secret");
// builder.Services.AddOpeniddictExtention(url, clientId, secret);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));


builder.Services.AddDbContextPool<CamsDataWriteContext>(option =>
{ 
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<CamsDataReadContext>(option =>
{ 
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<IpWhiteListedDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("IPDB")); //I set this on appsettings.json
});


builder.Services.AddCAMSServiceDependecy();


//ConfigureMassTransit(builder.Services);

//void ConfigureMassTransit(IServiceCollection services)
//{
//    builder.Services.AddMassTransit(c =>
//    {
//        c.UsingRabbitMq((context, cfg) =>
//        {
//            cfg.ConfigureEndpoints(context);

//            cfg.Host(builder.Configuration["QueueSetting:HostName"], builder.Configuration["QueueSetting:VirtualHost"], h =>
//            {
//                h.Username(builder.Configuration["QueueSetting:UserName"]);
//                h.Password(builder.Configuration["QueueSetting:Password"]);
//            });

//            cfg.ExchangeType = ExchangeType.Direct;
//        });

//    });

//    builder.Services.AddMassTransitHostedService();
//}
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(configuration.GetValue<string>("Url:baseUrl"))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

//builder.Services.AddCors(policy =>
//{
//    policy.AddPolicy("CorsPolicy", opt => opt
//        .WithOrigins("https://localhost:5001")
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials());
//});

builder.Services.AddSignalR();
builder.Services.AddSingleton<TimerManager>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFileServer();

app.UseCors(MyAllowSpecificOrigins); 
app.UseHttpsRedirection();

//app.UseMiddleware<ExceptionFormattingMiddleware>();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<ChartHub>("/Socket");
//});
app.MapHub<ChartHub>("/Socket/CallSocket");

app.Run();