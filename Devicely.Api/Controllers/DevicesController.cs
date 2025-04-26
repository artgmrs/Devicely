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
        /// <response code="204">No Devices Found</response>
        /// <response code="400">Invalid Request Data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public ActionResult<List<DeviceDto>> GetAllDevices([FromQuery] string? brand, [FromQuery] DeviceState? state, [FromQuery] PaginationDto paginationDto)
        {
            try
            {
                var devices = _deviceService.GetAllDevices(brand, state, paginationDto.PageSize, paginationDto.PageNumber);

                if (devices.Count == 0) return NoContent();

                return Ok(devices.ToDtoList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a device by its ID
        /// </summary>
        /// <param name="id">Device ID (int)</param>
        /// <response code="200">Successfully retrieved the device</response>
        /// <response code="204">Device not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeviceDto>> GetDeviceById([Required] int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceByIdAsync(id);

                if (device == null) return NoContent();

                return Ok(device.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="createDeviceDto">Device data</param>
        /// <response code="201">Device successfully created</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        public async Task<ActionResult<DeviceDto>> CreateDevice([FromBody] CreateDeviceDto createDeviceDto)
        {
            try
            {
                var createdDevice = await _deviceService.CreateDeviceAsync(createDeviceDto.ToEntity());

                return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id }, createdDevice.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing device
        /// </summary>
        /// <param name="id">Device ID (int)</param>
        /// <param name="editDeviceDto">Updated device data</param>
        /// <response code="200">Device successfully updated</response>
        /// <response code="204">Device not found</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<DeviceDto>> UpdateDevice([Required] int id, [FromBody] EditDeviceDto editDeviceDto)
        {
            try
            {
                var updatedDevice = await _deviceService.UpdateDeviceAsync(id, editDeviceDto.ToEntity(), editDeviceDto.State.HasValue);

                if (updatedDevice == null) return NoContent();

                return Ok(updatedDevice.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
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
            try
            {
                var deletedDevice = await _deviceService.DeleteDeviceByIdAsync(id);

                if (deletedDevice == null) return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
