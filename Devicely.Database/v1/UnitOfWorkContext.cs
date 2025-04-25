using Devicely.Database.Entities;
using Devicely.Database.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Database.v1;

public class UnitOfWorkContext(DbContextOptions<UnitOfWorkContext> options) : DbContext(options), IUnitOfWorkContext
{
    public DbSet<Device> Devices { get; set;}

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    // }a
}
