using Devicely.Database.Context;

namespace Devicely.Application.Services;

public class ServiceBase(DevicelyDbContext context)
{
    internal readonly DevicelyDbContext Context = context;
}
