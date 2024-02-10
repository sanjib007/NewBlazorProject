using L3TIdentityTokenServer.Extention;
using L3TIdentityTokenServer.Middleware;
using L3TIdentityTokenServer.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerExtention("Identity OAuth2 Token Server", "v1");
builder.Services.AddDBExtention(configuration);
builder.Services.AddIdentityExtention();
builder.Services.AddopenIddictExtention();
builder.Services.AddFluentValidationCollection();
builder.Services.AddServiceDependency();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionFormattingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();