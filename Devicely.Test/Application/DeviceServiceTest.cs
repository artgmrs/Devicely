using Devicely.Application.Services;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Devicely.Domain.Constants;
using Devicely.Domain.Enums;
using Devicely.Test.Configuration;

namespace Devicely.Test.Application;

public class DeviceServiceTest
{
    [Theory, AutoDomainData]
    public void GetAllDevices_ShouldReturnAllDevices(DeviceService sut)
    {
        var (result, totalCount) = sut.GetAllDevices(brand: null, state: null, PaginationConstants.DefaultPageSize, PaginationConstants.DefaultPageNumber);

        Assert.NotNull(result);
        Assert.All(result, device => Assert.False(device.IsDeleted));
    }

    [Theory, AutoDomainData]
    public void GetAllDevices_Filter_ShouldReturnDevicesByBrand(DeviceService sut)
    {
        var brand = "Brand A";

        var (result, totalCount) = sut.GetAllDevices(brand: brand, state: null, PaginationConstants.DefaultPageSize, PaginationConstants.DefaultPageNumber);

        Assert.NotNull(result);
        Assert.All(result, device => Assert.Equal(brand, device.Brand));
    }

    [Theory, AutoDomainData]
    public void GetAllDevices_Filter_ShouldReturnDevicesByState(DeviceService sut)
    {
        var state = DeviceState.Available;

        var (result, totalCount) = sut.GetAllDevices(brand: null, state: state, PaginationConstants.DefaultPageSize, PaginationConstants.DefaultPageNumber);

        Assert.NotNull(result);
        Assert.All(result, device => Assert.Equal(state, device.State));
    }

    [Theory, AutoDomainData]
    public void GetAllDevices_Filter_WhenNoDevicesMatch(DeviceService sut)
    {
        var (result, _) = sut.GetAllDevices("NonExistentBrand", null, PaginationConstants.DefaultPageSize, PaginationConstants.DefaultPageNumber);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory, AutoDomainData]
    public void GetAllDevices_Pagination_ShouldPaginate(DeviceService sut)
    {
        var (result, _) = sut.GetAllDevices(brand: null, state: null, 1, 1);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Theory, AutoDomainData]
    public async Task CreateDeviceAsync_ShouldCreateDeviceAsync(DeviceService sut, Device device)
    {
        device.Id = 0;
        var result = await sut.CreateDeviceAsync(device);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
        Assert.Equal(device.Name, result.Name);
        Assert.Equal(device.Brand, result.Brand);
        Assert.False(result.IsDeleted);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.UpdatedAt <= DateTime.UtcNow);
    }

    [Theory, AutoDomainData]
    public async Task GetDeviceByIdAsync_ShouldReturnDevice_WhenDeviceExistsAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.First();

        var result = await sut.GetDeviceByIdAsync(existingDevice.Id);

        Assert.NotNull(result);
        Assert.Equal(existingDevice.Id, result.Id);
        Assert.Equivalent(existingDevice, result);
    }

    [Theory, AutoDomainData]
    public async Task GetDeviceByIdAsync_ShouldReturnNull_WhenDeviceDoesNotExistAsync(DeviceService sut)
    {
        var result = await sut.GetDeviceByIdAsync(-1);

        Assert.Null(result);
    }

    [Theory, AutoDomainData]
    public async Task UpdateDeviceAsync_ShouldUpdateDeviceAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.Where(d => d.State == DeviceState.Available).First();
        var toUpdateDevice = new Device
        {
            Name = $"{existingDevice.Name} + Updated Name",
            Brand = $"{existingDevice.Name} + Updated Brand",
            State = DeviceState.InUse,
        };

        var result = await sut.UpdateDeviceAsync(existingDevice.Id, toUpdateDevice, true);

        Assert.NotNull(result);
        Assert.Equal(toUpdateDevice.Name, result.Name);
        Assert.Equal(toUpdateDevice.Brand, result.Brand);
        Assert.Equal(toUpdateDevice.State, result.State);
        Assert.True(result.CreatedAt != result.UpdatedAt);
    }

    [Theory, AutoDomainData]
    public async Task UpdateDeviceAsync_ShouldUpdateDeviceWithoutModifyingCreatedAtAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.Where(d => d.State == DeviceState.Available).First();
        var toUpdateDevice = new Device
        {
            State = DeviceState.InUse,
        };

        var result = await sut.UpdateDeviceAsync(existingDevice.Id, toUpdateDevice, true);

        Assert.NotNull(result);
        Assert.Equal(toUpdateDevice.CreatedAt, result.CreatedAt);
        Assert.True(result.CreatedAt != result.UpdatedAt);
    }

    [Theory, AutoDomainData]
    public async Task UpdateDeviceAsync_ShouldReturnNull_WhenDeviceDoesNotExistAsync(DeviceService sut, Device updatedDevice)
    {
        var result = await sut.UpdateDeviceAsync(-1, updatedDevice, true);

        Assert.Null(result);
    }

    [Theory, AutoDomainData]
    public async Task UpdateDeviceAsync_ShouldNotUpdateWhenNotModifyingAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.Where(d => d.State == DeviceState.Available).First();

        var result = await sut.UpdateDeviceAsync(existingDevice.Id, existingDevice, false);

        Assert.NotNull(result);
        Assert.Equal(existingDevice.UpdatedAt, result.UpdatedAt);
    }

    [Theory, AutoDomainData]
    public async Task UpdateDeviceAsync_ShouldNotUpdate_InUseValidationAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.Where(d => d.State == DeviceState.InUse).First();
        var toUpdateDevice = new Device
        {
            Name = $"{existingDevice.Name} + Updated Name",
            Brand = $"{existingDevice.Name} + Updated Brand",
        };

        await Assert.ThrowsAsync<Exception>(async () => await sut.UpdateDeviceAsync(existingDevice.Id, toUpdateDevice, true));
    }

    [Theory, AutoDomainData]
    public async Task DeleteDeviceByIdAsync_ShouldMarkDeviceAsDeletedAsync(DeviceService sut, DevicelyDbContext context, Device device)
    {
        device.IsDeleted = false;
        context.Devices.Add(device);
        device.Id = 0;
        await context.SaveChangesAsync();

        var result = await sut.DeleteDeviceByIdAsync(device.Id);

        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
    }

    [Theory, AutoDomainData]
    public async Task DeleteDeviceByIdAsync_ShouldReturnNull_DeviceAlreadyDeletedOrNotFoundAsync(DeviceService sut, DevicelyDbContext context)
    {
        var existingDevice = context.Devices.Where(d => d.IsDeleted).First();

        var result = await sut.DeleteDeviceByIdAsync(existingDevice.Id);

        Assert.Null(result);
    }

    [Theory, AutoDomainData]
    public async Task DeleteDeviceByIdAsync_ShouldReturnNull_WhenDeviceDoesNotExistAsync(DeviceService sut)
    {
        var result = await sut.DeleteDeviceByIdAsync(199);

        Assert.Null(result);
    }

    [Theory, AutoDomainData]
    public async Task DeleteDeviceByIdAsync_ShouldThrowException_WhenDeviceIsInUseAsync(DeviceService sut, DevicelyDbContext context, Device device)
    {
        device.State = DeviceState.InUse;
        device.IsDeleted = false;
        device.Id = 0;
        context.Devices.Add(device);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<Exception>(async () => await sut.DeleteDeviceByIdAsync(device.Id));
    }
}