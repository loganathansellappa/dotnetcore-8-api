using Microsoft.OpenApi.Models;
using Restaurants.API.Controllers;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
                
        // Add services to the container.

        builder.Services.AddEndpointsApiExplorer();

        // Add Authentication
        builder.Services.AddAuthentication();

        builder.Services.AddControllers();
        builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http, 
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    []
                }
            });
        });
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<LoggingMiddleware>();
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
    }
}