using Devicely.Database.Entities;
using Devicely.Domain.Enums;

namespace Devicely.Application.Interfaces;

public interface IDeviceService
{
    List<Device> GetAllDevices(string? brand, DeviceState? state, int pageSize, int pageNumber);
    Task<Device?> GetDeviceByIdAsync(Guid id);
    Task<Device> CreateDeviceAsync(Device device);
    Task<Device?> UpdateDeviceAsync(Guid id, Device device, bool updateState);
}
