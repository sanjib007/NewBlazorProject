using Microsoft.EntityFrameworkCore;
using L3T.Infrastructure.Helpers.DataContext.OltDBContext;
using MediatR;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

builder.Services.AddDbContextPool<OltDataWriteContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<OltDataReadContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});

builder.Services.AddOltServiceDependency();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
