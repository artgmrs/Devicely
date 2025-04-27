using Devicely.Application.Interfaces;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Devicely.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Application.Services;

public class DeviceService(DevicelyDbContext context) : ServiceBase(context), IDeviceService
{
    public (List<Device> result, int totalCount) GetAllDevices(string? brand, DeviceState? state, int pageSize, int pageNumber)
    {
        var query = Context.Devices.AsQueryable();

        if (!string.IsNullOrEmpty(brand))
            query = query.Where(d => d.Brand == brand);

        if (state.HasValue)
            query = query.Where(d => d.State == state);

        query = query.Where(d => !d.IsDeleted);
        var totalCount = query.Count();

        var result = query.AsNoTracking()
            .OrderBy(d => d.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (result, totalCount);
    }

    public async Task<Device> CreateDeviceAsync(Device device)
    {
        var date = DateTime.UtcNow;
        device.CreatedAt = date;
        device.UpdatedAt = date;
        device.IsDeleted = false;

        await Context.Devices.AddAsync(device);
        await Context.SaveChangesAsync();

        return device;
    }

    public async Task<Device?> GetDeviceByIdAsync(int id) => await Context.Devices.FindAsync(id);

    public async Task<Device?> UpdateDeviceAsync(int id, Device request, bool updateState)
    {
        var existingDevice = await Context.Devices.FindAsync(id);

        if (existingDevice == null) return null;

        ValidateNameBrandUpdate(existingDevice.State, request.Name, request.Brand);

        existingDevice.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : existingDevice.Name;
        existingDevice.Brand = !string.IsNullOrEmpty(request.Brand) ? request.Brand : existingDevice.Brand;

        if (updateState)
            existingDevice.State = request.State;

        if (request.IsDeleted != existingDevice.IsDeleted)
            existingDevice.IsDeleted = request.IsDeleted;

        if (!Context.Entry(existingDevice).Properties.Any(p => p.IsModified)) return existingDevice;

        existingDevice.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return existingDevice;
    }

    public async Task<Device?> DeleteDeviceByIdAsync(int id)
    {
        var existingDevice = Context.Devices.Where(d => d.Id == id && !d.IsDeleted).FirstOrDefault();

        if (existingDevice == null) return null;

        existingDevice.IsDeleted = true;
        await Context.SaveChangesAsync();

        return existingDevice;
    }

    // Can't update name or brand when state is InUse
    private static void ValidateNameBrandUpdate(DeviceState state, string name, string brand)
    {
        if (state == DeviceState.InUse && (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(brand)))
            throw new Exception("Device name and or brand can't be updated when in device is in use.");
    }
}
