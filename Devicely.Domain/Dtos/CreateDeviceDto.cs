using System.ComponentModel.DataAnnotations;
using Devicely.Domain.Enums;

namespace Devicely.Domain.Dtos;

public class CreateDeviceDto
{

    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Brand { get; set; } = string.Empty;
    [Required]
    public DeviceState State { get; set; }
}
