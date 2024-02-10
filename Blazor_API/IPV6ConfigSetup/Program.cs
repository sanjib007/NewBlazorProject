using IPV6ConfigSetup.Extention;
using IPV6ConfigSetup.Pagination;
using IPV6ConfigSetup.PipelineBehaviours;
using IPV6ConfigSetup.Repositories;
using IPV6ConfigSetup.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtention("IPV6 Configuration Setup", "v1");
builder.Services.AddDBExtention(configuration);
builder.Services.AddServiceDependency();
builder.Services.AddRepositoryDependency();
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





// pagination service start
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});
// pagination service End

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

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddAutoMapper(typeof(Program));




var app = builder.Build();

// Configure the HTTP request pipeline. IsShowSwagger
if (configuration.GetValue<bool>("IsShowSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
