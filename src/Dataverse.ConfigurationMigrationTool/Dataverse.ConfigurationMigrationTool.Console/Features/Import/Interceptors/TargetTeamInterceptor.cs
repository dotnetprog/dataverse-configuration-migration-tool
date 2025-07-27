using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class TargetTeamInterceptor : BaseEntityInterceptor
{
    private readonly ITeamRepository _teamRepository;
    public TargetTeamInterceptor(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }
    public override async Task<Entity> InterceptAsync(Entity entity)
    {
        var attributes = entity.Attributes.Where(kv => kv.Value is EntityReference ef && ef.LogicalName == "team").ToList();
        foreach (var attribute in attributes)
        {
            var reference = attribute.Value as EntityReference;
            var resolvedTeam = await _teamRepository.GetByIdAsync(reference.Id);
            if (resolvedTeam != null)
            {
                continue;

            }
            resolvedTeam = await _teamRepository.GetByNameAsync(reference.Name);
            if (resolvedTeam != null)
            {
                entity[attribute.Key] = new EntityReference("team", resolvedTeam.Id);

            }
            else
            {
                entity.Attributes.Remove(attribute.Key);
            }
        }
        return await base.InterceptAsync(entity);
    }
}
