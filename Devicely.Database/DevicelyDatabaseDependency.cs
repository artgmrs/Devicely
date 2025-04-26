using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Devicely.Database.Context;

namespace Devicely.Database;

public static class DevicelyDatabaseDependency
{
    public static void AddDevicelyDatabaseModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DevicelyDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DevicelyDbContext>();

        Console.WriteLine("Applying migrations...");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
}
