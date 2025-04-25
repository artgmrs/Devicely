using Devicely.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Devicely.Database.Interfaces.v1;

public interface IUnitOfWorkContext
{
    /// <summary>
    /// Device
    /// </summary>
    DbSet<Device> Devices { get; }
}
