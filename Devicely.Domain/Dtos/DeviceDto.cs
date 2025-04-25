using Devicely.Domain.Enums;

namespace Devicely.Domain.Dtos;

public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DeviceState State { get; set; }
    public DateTime CreatedAt { get; set; }
}
