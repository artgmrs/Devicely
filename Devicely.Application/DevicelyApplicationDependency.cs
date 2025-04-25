using Devicely.Application.Interfaces;
using Devicely.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Devicely.Application;

public static class DevicelyApplicationDependency
{
    public static void AddDevicelyApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
    }
}
