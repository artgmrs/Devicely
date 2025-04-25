using Devicely.Database.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Devicely.Database;

public static class DevicelyDatabaseDependency
{
    public static void AddDevicelyDatabaseModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UnitOfWorkContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
