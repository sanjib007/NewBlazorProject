using L3T.Infrastructure.Helpers.DataContext.ScheduleDBContext;
using L3T.Infrastructure.Helpers.ServiceDependencies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContextPool<ScheduleDataContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("WFA2131DBConection")); //I set this on appsettings.json
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScheduledServiceDependency();

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
