using Devicely.Domain.Enums;

namespace Devicely.Domain.Dtos;

public class EditDeviceDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DeviceState? State { get; set; }
    public bool IsDeleted { get; set; }
}
