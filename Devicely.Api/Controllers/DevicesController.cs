using System.ComponentModel.DataAnnotations;
using Devicely.Api.Mappings;
using Devicely.Application.Interfaces;
using Devicely.Domain.Dtos;
using Devicely.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Devicely.Api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController(
        IDeviceService deviceService,
        ILogger<DevicesController> logger) : ControllerBase
    {
        private readonly IDeviceService _deviceService = deviceService;
        private readonly ILogger<DevicesController> _logger = logger;

        /// <summary>
        /// Get devices with filter and pagination (optionals)
        /// </summary>
        /// <param name="brand">Brand Name</param>
        /// <param name="state">1- Available, 2- InUse, 3- Inactive</param>
        /// <param name="paginationDto">Pagination Parameters</param>
        /// <response code="200">Successfully Retrieved Data</response>
        /// <response code="400">Invalid Request Data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public ActionResult<PaginatedResultDto<DeviceDto>> GetAllDevices([FromQuery] string? brand, [FromQuery] DeviceState? state, [FromQuery] PaginationDto paginationDto)
        {
            var (devices, totalCount) = _deviceService.GetAllDevices(brand, state, paginationDto.PageSize, paginationDto.PageNumber);

            return Ok(devices.ToPaginatedResultDto(totalCount, paginationDto.PageNumber, paginationDto.PageSize));
        }

        /// <summary>
        /// Get a device by its ID
        /// </summary>
        /// <param name="id">Device ID (int)</param>
        /// <response code="200">Successfully retrieved the device</response>
        /// <response code="404">Device not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeviceDto>> GetDeviceById([Required] int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);

            if (device == null) return NotFound();

            return Ok(device.ToDto());
        }

        /// <summary>
        /// Create a new device 
        /// </summary>
        /// <param name="createDeviceDto">Device data</param>
        /// <remarks>
        /// Example request:
        /// 
        ///     POST /api/devices
        ///     {
        ///         "name": "Example Device",
        ///         "brand": "Example Brand",
        ///         "state": 1 // Valid values for state are: 1 (Available), 2 (InUse), and 3 (Inactive)
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Device successfully created</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] CreateDeviceDto createDeviceDto)
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(createDeviceDto.ToEntity());

            return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id }, createdDevice.ToDto());
        }

        /// <summary>
        /// Update an existing device
        /// </summary>
        /// <param name="id">Device ID (int)</param>
        /// <param name="editDeviceDto">Updated device data</param>
        /// <response code="200">Device successfully updated</response>
        /// <response code="404">Device not found</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<DeviceDto>> UpdateDevice([Required] int id, [FromBody] EditDeviceDto editDeviceDto)
        {
            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, editDeviceDto.ToEntity(), editDeviceDto.State.HasValue);

            if (updatedDevice == null) return NotFound();

            return Ok(updatedDevice.ToDto());
        }

        /// <summary>
        /// Delete a device by its ID
        /// </summary>
        /// <param name="id">Device ID (int)</param>
        /// <response code="200">Device successfully deleted</response>
        /// <response code="404">Device not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDevice([Required] int id)
        {
            var deletedDevice = await _deviceService.DeleteDeviceByIdAsync(id);

            if (deletedDevice == null) return NotFound();

            return NoContent();
        }
    }
}
