using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
public interface IBusinessUnitRepository
{
    Task<Entity> GetByNameAsync(string businessUnitName);
    Task<Entity> GetByIdAsync(Guid Id);
}
