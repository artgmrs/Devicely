using Devicely.Domain.Dtos;
using Devicely.Domain.Enums;

namespace Devicely.Database.Entities;

public class Device
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DeviceState State { get; set; }
    public DateTime CreatedAt { get; set; }
}
