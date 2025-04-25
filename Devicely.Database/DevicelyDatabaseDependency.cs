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
}
