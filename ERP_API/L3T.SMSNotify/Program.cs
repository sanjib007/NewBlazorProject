using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.SMSNotifyDBContext;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var configuration = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
/*var url = configaretion.GetValue<string>("Url:Auth");
var clientId = configaretion.GetValue<string>("Url:ClientId");
var secret = configaretion.GetValue<string>("Url:Secret");*/
//builder.Services.AddOpeniddictExtention(url, clientId, secret);

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));



builder.Services.AddDbContextPool<SMSNotifyWriteDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<SMSNotify131DBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB131")); //I set this on appsettings.json
});

var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));

// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<SMSNotifyReadDBContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration.GetConnectionString("ReadDB"), serverVersion)
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

builder.Services.AddSMSNotifyServiceDependecy();



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




builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

//app.UseMiddleware<ExceptionFormattingMiddleware>();
/*app.UseAuthentication();
app.UseAuthorization();*/

app.MapControllers();

app.Run();
