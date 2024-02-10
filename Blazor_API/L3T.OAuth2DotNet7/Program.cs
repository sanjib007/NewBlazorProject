

using L3T.OAuth2DotNet7.Extention;
using L3T.OAuth2DotNet7.Middleware;
using L3T.OAuth2DotNet7.Pagination;
using L3T.OAuth2DotNet7.PipelineBehaviours;
using L3T.OAuth2DotNet7.Repositories;
using L3T.OAuth2DotNet7.Services;
using MediatR;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerExtention("Identity OAuth2 Server .net7", "v1");
builder.Services.AddDBExtention(configuration);
builder.Services.AddIdentityExtention();
builder.Services.AddopenIddictExtention(configuration);
builder.Services.AddFluentValidationCollection();
builder.Services.AddServiceDependency();
builder.Services.AddRepositoryDependency();
builder.Services.AddHttpClient();
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
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddHostedService<TestData>();






var app = builder.Build();

// Configure the HTTP request pipeline.

if (configuration.GetValue<bool>("IsShowSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<ExceptionFormattingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
