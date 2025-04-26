using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Devicely.Database.Context;
using Devicely.Database.Entities;
using Devicely.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Test.Configuration;

public class AutoDomainDataAttribute : AutoDataAttribute
{
    public AutoDomainDataAttribute()
    : base(() =>
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        
        // Configure EF InMemoryDatabase to use in tests
        var options = new DbContextOptionsBuilder<DevicelyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new DevicelyDbContext(options);

        // Seed data
        if (!context.Devices.Any())
        {
            context.Devices.AddRange(new List<Device>
            {
                new Device { Id = 1, Name = "Device 1", Brand = "Brand A", State = DeviceState.Available, IsDeleted = false },
                new Device { Id = 2, Name = "Device 2", Brand = "Brand B", State = DeviceState.InUse, IsDeleted = false },
                new Device { Id = 3, Name = "Device 3", Brand = "Brand C", State = DeviceState.InUse, IsDeleted = true },
                new Device { Id = 4, Name = "Device 4", Brand = "Brand A", State = DeviceState.Available, IsDeleted = false },
                new Device { Id = 5, Name = "Device 5", Brand = "Brand B", State = DeviceState.Inactive, IsDeleted = false },
                new Device { Id = 6, Name = "Device 6", Brand = "Brand C", State = DeviceState.Available, IsDeleted = false },
                new Device { Id = 7, Name = "Device 7", Brand = "Brand A", State = DeviceState.Inactive, IsDeleted = false },
                new Device { Id = 8, Name = "Device 8", Brand = "Brand B", State = DeviceState.Available, IsDeleted = false },
                new Device { Id = 9, Name = "Device 9", Brand = "Brand C", State = DeviceState.Inactive, IsDeleted = false },
                new Device { Id = 10, Name = "Device 10", Brand = "Brand A", State = DeviceState.Available, IsDeleted = false }
            });
        }
        
        context.SaveChanges();

        fixture.Inject(context);
        
        return fixture;
    })
    {}
}
