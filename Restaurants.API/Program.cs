using Restaurants.API.Controllers;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    /*
     * Move to appsettings.json
     */
    //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    //.Enrich.FromLogContext()
    //.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
    //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
    //
});
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

await seeder.Seed();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
