using Devicely.Application.Interfaces;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Devicely.Application.Services;

public class DeviceService(
    ILogger<DeviceService> logger,
    DevicelyDbContext context) : ServiceBase(context), IDeviceService 
{
    private readonly ILogger<DeviceService> logger = logger;

    public async Task<List<Device>> GetAllDevices()
    {
        try
        {
            return await Context.Devices.ToListAsync();
        }
        catch (Exception ex)
        {
            var message = $"Error ocurred when querying all devices: {ex.Message}";
            logger.LogError(message, ex);
            throw new Exception(message, ex); // criar DeviceException
        }
    }
}
