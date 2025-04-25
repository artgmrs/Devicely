using Devicely.Application.Interfaces;
using Devicely.Database.Entities;
using Devicely.Database.v1;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Application.Services;

public class DeviceService(UnitOfWorkContext unitOfWorkContext) : ServiceBase(unitOfWorkContext), IDeviceService 
{
    public async Task<List<Device>> GetAllDevices()
    {
        var result = await UnitOfWorkContext.Devices.ToListAsync();

        return result;
    }
}
