using Microsoft.EntityFrameworkCore;
using MediatR;
using L3T.Infrastructure.Helpers.DataContext.LocationDBContext;
using L3T.Infrastructure.Helpers.ServiceDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddDbContextPool<LocationDataWriteContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<LocationDataReadContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});

builder.Services.AddLocationServiceDependency();

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
