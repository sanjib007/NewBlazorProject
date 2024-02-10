using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.DataContext.BtsDBContext;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
//builder.Host.UseSerilog();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
// Add the memory cache services.
builder.Services.AddMemoryCache();

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContextPool<BtsDataWriteContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<BtsDataReadContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});


builder.Services.AddBtsServiceDependency();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization(); 
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

app.MapControllers();


app.Run();

//app.Run();
// EntityFrameworkCore\Add-Migration init -Context BtsDataWriteContext
// EntityFrameworkCore\Update-Database -Context BtsDataWriteContext