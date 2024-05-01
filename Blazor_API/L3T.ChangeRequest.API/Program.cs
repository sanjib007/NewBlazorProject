using L3T.ChangeRequest.API.Extention;
using L3T.ChangeRequest.API.Middleware;
using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.MailConfiguration;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Repositories;
using L3T.Infrastructure.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

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

var url = configuration.GetValue<string>("OpnIdUrl:Auth");
var clientId = configuration.GetValue<string>("OpnIdUrl:ClientId");
var secret = configuration.GetValue<string>("OpnIdUrl:Secret");

builder.Services.AddOpeniddictExtention(url, clientId, secret);

builder.Services.AddAutoMapper(typeof(Program));

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddDbContextPool<ChangeRequestDataContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlDB")); // Set this on appsettings.json
});

builder.Services.AddDbContextPool<ApplicationMenuAndRoleWiseMenuPermissionDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PermissionDB")); // Set this on appsettings.json
});

//builder.Services.AddDbContextPool<LnkDbContext>(option =>
//{
//    option.UseSqlServer(builder.Configuration.GetConnectionString("LNKDB")); // Set this on appsettings.json
//});

builder.Services.AddServicesDependency();
builder.Services.AddRepositoriesDependency();

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

//builder.Services.AddHttpClient();
var apiGateWayUrl = configuration.GetValue<string>("Url:baseUrl");
builder.Services.AddHttpClient("apiGateway", httpClient =>
{
	httpClient.BaseAddress = new Uri(apiGateWayUrl);
});
builder.Services.AddHttpContextAccessor();

// pagination service start
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});

builder.Services.Configure<EmailSetting>(configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<PermissionValidationMiddleware>();


var app = builder.Build();

if (configuration.GetValue<bool>("SwaggerAndPathSetup:IsSetPathBase"))
{
    app.UsePathBase(configuration.GetValue<string>("SwaggerAndPathSetup:SetPathBase"));
}

// Configure the HTTP request pipeline.
if (configuration.GetValue<bool>("SwaggerAndPathSetup:IsShowSwagger"))
{
    if (configuration.GetValue<bool>("SwaggerAndPathSetup:IsShowSwaggerConfig"))
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = configuration.GetValue<string>("SwaggerAndPathSetup:RouteTemplate");
        });


        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(configuration.GetValue<string>("SwaggerAndPathSetup:SwaggerPath"), 
                configuration.GetValue<string>("SwaggerAndPathSetup:SwaggerName"));
            c.RoutePrefix = configuration.GetValue<string>("SwaggerAndPathSetup:RoutePrefix");
        });
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PermissionValidationMiddleware>();

app.MapControllers();

app.Run();
