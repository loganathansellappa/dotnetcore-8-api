using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Seeders;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddPresentation();

    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

    await seeder.Seed();
    await seeder.SeedRandomDataWithHardcodedUser(5000);

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<LoggingMiddleware>();
// Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();


    app.UseSwagger();


    app.UseHttpsRedirection();
// EF Identity Endpoints
    app.MapGroup("api/identity")
        .WithTags("Identity") // for swagger to display custom user endpoints in group with default generated ones
        .MapIdentityApi<User>();
    app.UseAuthorization();

    app.MapControllers();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();   
        app.UseSwaggerUI();
    }
    app.Run();
} catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}


public partial class Program { }