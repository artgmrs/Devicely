using Devicely.Database.Entities;

namespace Devicely.Application.Interfaces;

public interface IDeviceService
{
    Task<List<Device>> GetAllDevices();
}
