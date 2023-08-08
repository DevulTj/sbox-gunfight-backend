using Api.Services;
using Base;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();

if ( builder.Environment.IsDevelopment() )
{
	builder.Configuration.AddEnvironmentVariables().AddJsonFile( "appsettings.Development.json" );
	DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString( "AZURE_SQL_CONNECTIONSTRING" );
}
else
{
	DatabaseContext.ConnectionString = Environment.GetEnvironmentVariable( "AZURE_SQL_CONNECTIONSTRING" );
}

builder.Services.AddDbContext<DatabaseContext>( ServiceLifetime.Scoped, ServiceLifetime.Scoped );

// Services
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IMatchService, MatchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

	Global.IsDevelopment = true;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.MapControllers();

app.Run();
