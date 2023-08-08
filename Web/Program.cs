using Api.Services;
using Base;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddRazorPages();

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
if ( !app.Environment.IsDevelopment() )
{
	app.UseExceptionHandler( "/Error" );
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
