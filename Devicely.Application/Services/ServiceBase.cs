using Devicely.Database.Interfaces.v1;
using Devicely.Database.v1;

namespace Devicely.Application.Services;

public class ServiceBase(UnitOfWorkContext unitOfWorkContext)
{
    internal readonly UnitOfWorkContext UnitOfWorkContext = unitOfWorkContext;
}
