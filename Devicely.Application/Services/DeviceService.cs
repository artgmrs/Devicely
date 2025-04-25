using Devicely.Application.Interfaces;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Application.Services;

public class DeviceService(DevicelyDbContext context) : ServiceBase(context), IDeviceService 
{
    public async Task<List<Device>> GetAllDevices()
    {
        var result = await Context.Devices.ToListAsync();

        return result;
    }
}
