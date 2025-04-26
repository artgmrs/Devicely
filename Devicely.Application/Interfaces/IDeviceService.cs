using Devicely.Database.Entities;
using Devicely.Domain.Enums;

namespace Devicely.Application.Interfaces;

public interface IDeviceService
{
    List<Device> GetAllDevices(string? brand, DeviceState? state, int pageSize, int pageNumber);
    Task<Device?> GetDeviceByIdAsync(int id);
    Task<Device?> DeleteDeviceByIdAsync(int id);
    Task<Device> CreateDeviceAsync(Device device);
    Task<Device?> UpdateDeviceAsync(int id, Device device, bool updateState);
}
