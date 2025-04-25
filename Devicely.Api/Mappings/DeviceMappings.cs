using Devicely.Database.Entities;
using Devicely.Domain.Dtos;

namespace Devicely.Api.Mappings;

public static class DeviceMappings
{
    // Converte um Device para DeviceDto
    public static DeviceDto ToDto(this Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            Brand = device.Brand,
            State = device.State,
            CreatedAt = device.CreatedAt
        };
    }

    // Converte um DeviceDto para Device
    public static Device ToEntity(this DeviceDto deviceDto)
    {
        return new Device
        {
            Id = deviceDto.Id,
            Name = deviceDto.Name,
            Brand = deviceDto.Brand,
            State = deviceDto.State,
            CreatedAt = deviceDto.CreatedAt
        };
    }
}
