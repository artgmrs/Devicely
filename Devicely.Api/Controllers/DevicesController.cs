using Devicely.Api.Mappings;
using Devicely.Application.Interfaces;
using Devicely.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Devicely.Api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController(IDeviceService deviceService) : ControllerBase
    {
        private readonly IDeviceService _deviceService = deviceService;

        [HttpGet]
        public async Task<ActionResult<List<DeviceDto>>> GetAllDevices()
        {
            var devices = await _deviceService.GetAllDevices();

            if (devices.Count == 0) return NoContent();

            return Ok(devices.ToDtoList());
        }
    }
}
