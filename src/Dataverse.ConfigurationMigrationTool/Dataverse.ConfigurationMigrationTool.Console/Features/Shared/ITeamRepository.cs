using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
public interface ITeamRepository
{
    Task<Entity> GetByNameAsync(string teamName);
    Task<Entity> GetByIdAsync(Guid Id);
}
