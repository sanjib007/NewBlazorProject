using L3T.FieldForceApi.Extention;
using L3T.FieldForceApi.Middleware;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.MailConfiguration;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Serilog;




var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _hostingEnv = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var configuration = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
var url = configuration.GetValue<string>("Url:Auth");
var clientId = configuration.GetValue<string>("Url:ClientId");
var secret = configuration.GetValue<string>("Url:Secret");
builder.Services.AddOpeniddictExtention(url, clientId, secret);

builder.Services.AddFluentValidationCollectionExtenstion();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));


builder.Services.AddDbContextPool<FFWriteDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<FFReadDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<MisDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("MisConnection")); //I set this on appsettings.json
});

builder.Services.AddDbContextPool<RsmDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("RsmConnection")); //I set this on appsettings.json
});

builder.Services.AddDbContextPool<HydraDBContext>(option =>
{
    option.UseOracle(builder.Configuration.GetConnectionString("HydraDBConnection")); //I set this on appsettings.json
});

builder.Services.AddDbContextPool<L3TDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("L3TConnection")); //I set this on appsettings.json
});



builder.Services.AddFFServiceDependecy();
builder.Services.AddOtherServiceDependency();
builder.Services.AddHttpClient();


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

builder.Services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwaggerAuthorized();
}
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

//app.UseMiddleware<ExceptionFormattingMiddleware>();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
