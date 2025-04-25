using Devicely.Application.Interfaces;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Devicely.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Devicely.Application.Services;

public class DeviceService(
    ILogger<DeviceService> logger,
    DevicelyDbContext context) : ServiceBase(context), IDeviceService
{
    private readonly ILogger<DeviceService> logger = logger;

    public List<Device> GetAllDevices(string? brand, DeviceState? state, int pageSize, int pageNumber)
    {
        try
        {
            var query = Context.Devices.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(d => d.Brand == brand);

            if (state.HasValue) // confirmar que funciona
                query = query.Where(d => d.State == state);

            return query.AsNoTracking()
                .OrderBy(d => d.CreatedAt) //@todo - trocar pra id 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        catch (Exception ex)
        {
            var message = $"Error ocurred when querying all devices: {ex.Message}";
            logger.LogError(message, ex);
            throw new Exception(message, ex);
        }
    }

    public async Task<Device> CreateDeviceAsync(Device device)
    {
        try
        {
            device.CreatedAt = DateTime.UtcNow;
            await Context.Devices.AddAsync(device);
            await Context.SaveChangesAsync();

            return device;
        }
        catch (Exception ex)
        {
            var message = $"Error occurred when creating a device: {ex.Message}";
            logger.LogError(message, ex);
            throw new Exception(message, ex);
        }
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid id)
    {
        try
        {
            return await Context.Devices.FindAsync(id);
        }
        catch (Exception ex)
        {
            var message = $"Error occurred when retrieving device by ID: {ex.Message}";
            logger.LogError(message, ex);
            throw new Exception(message, ex);
        }
    }

    public async Task<Device?> UpdateDeviceAsync(Guid id, Device request, bool updateState)
    {
        try
        {
            var existingDevice = await Context.Devices.FindAsync(id);

            if (existingDevice == null) return null;

            if (existingDevice.State == DeviceState.InUse
                && (!string.IsNullOrEmpty(request.Name) || !string.IsNullOrEmpty(request.Brand)))
                throw new Exception("Device name and or brand can't be updated when in use.");

            if (!string.IsNullOrEmpty(request.Name))
                existingDevice.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Brand))
                existingDevice.Brand = request.Brand;

            if (updateState)
                existingDevice.State = request.State;

            await Context.SaveChangesAsync();

            return existingDevice;
        }
        catch (Exception ex)
        {
            var message = $"Error occurred when updating device: {ex.Message}";
            logger.LogError(message, ex);
            throw new Exception(message, ex);
        }
    }
}
