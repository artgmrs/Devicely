using Devicely.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Database.Context;

public class DevicelyDbContext(DbContextOptions<DevicelyDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
