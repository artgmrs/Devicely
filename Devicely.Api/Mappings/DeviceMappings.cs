using Devicely.Database.Entities;
using Devicely.Domain.Dtos;

namespace Devicely.Api.Mappings;

public static class DeviceMappings
{
    // Converts a Device to DeviceDto
    public static DeviceDto ToDto(this Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            State = device.State,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt,
            IsDeleted = device.IsDeleted
        };
    }

    // Converts a list of Device to a list of DeviceDto
    public static List<DeviceDto> ToDtoList(this List<Device> devices)
    {
        return devices.Select(device => device.ToDto()).ToList();
    }

    // Converts a DeviceDto to Device
    public static Device ToEntity(this DeviceDto deviceDto)
    {
        return new Device
        {
            Id = deviceDto.Id,
            Name = deviceDto.Name,
            Brand = deviceDto.Brand,
            State = deviceDto.State,
            CreatedAt = deviceDto.CreatedAt,
            UpdatedAt = deviceDto.UpdatedAt,
            IsDeleted = deviceDto.IsDeleted
        };
    }

    // Converts a list of DeviceDto to a list of Device
    public static List<Device> ToEntityList(this List<DeviceDto> devices)
    {
        return devices.Select(device => device.ToEntity()).ToList();
    }

    // Converts a CreateDeviceDto to Device
    public static Device ToEntity(this CreateDeviceDto createDeviceDto)
    {
        return new Device
        {
            Name = createDeviceDto.Name,
            Brand = createDeviceDto.Brand,
            State = createDeviceDto.State,
        };
    }

    // Converts an EditDeviceDto to Device
    public static Device ToEntity(this EditDeviceDto editDeviceDto)
    {
        var device = new Device()
        {
            Name = editDeviceDto.Name,
            Brand = editDeviceDto.Brand,
            IsDeleted = editDeviceDto.IsDeleted,
        };

        if (editDeviceDto.State.HasValue)
        {
            device.State = editDeviceDto.State.Value;
        }

        return device;
    }
}
