using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.DataContext.OracleDBContex;
using L3T.Infrastructure.Helpers.DataContext.SelfCareDbContext;
using L3T.Infrastructure.Helpers.PipelineBehaviours;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using L3T.SelfcareAPI.Extention;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var url = configuration.GetValue<string>("Url:Auth");
var clientId = configuration.GetValue<string>("Url:ClientId");
var secret = configuration.GetValue<string>("Url:Secret");
builder.Services.AddOpeniddictExtention(url, clientId, secret);


builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

builder.Services.AddDbContextPool<SelfCareWriteDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WriteDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<SelfCareReadDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ReadDB")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<Wfa2_133DBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Wfa2_133")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<L3T_131DBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("L3T_131")); //I set this on appsettings.json
});
builder.Services.AddDbContextPool<WFA2_131DBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("WFA2_131")); //I set this on appsettings.json
});

builder.Services.AddDbContext<OraDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("OraDbConnection"));
});


builder.Services.AddSelfCareServiceDependecy();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
