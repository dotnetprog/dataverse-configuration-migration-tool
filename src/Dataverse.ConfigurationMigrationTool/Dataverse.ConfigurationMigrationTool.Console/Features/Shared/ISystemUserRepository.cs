using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
public interface ISystemUserRepository
{
    Task<Entity> GetByName(string fullname);
    Task<Entity> GetByIdAsync(Guid Id);
}
