using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class TargetUserInterceptor : BaseEntityInterceptor
{
    private readonly ISystemUserRepository _systemUserRepository;
    public TargetUserInterceptor(ISystemUserRepository systemUserRepository)
    {
        _systemUserRepository = systemUserRepository;
    }
    public override async Task<Entity> InterceptAsync(Entity entity)
    {
        var attributes = entity.Attributes.Where(kv => kv.Value is EntityReference ef && ef.LogicalName == "systemuser").ToList();
        foreach (var attribute in attributes)
        {
            var reference = attribute.Value as EntityReference;
            var resolvedUser = await _systemUserRepository.GetByIdAsync(reference.Id);
            if (resolvedUser != null)
            {
                continue;

            }
            resolvedUser = await _systemUserRepository.GetByName(reference.Name);
            if (resolvedUser != null)
            {
                entity[attribute.Key] = new EntityReference("systemuser", resolvedUser.Id);

            }
            else
            {
                entity.Attributes.Remove(attribute.Key);
            }
        }
        return await base.InterceptAsync(entity);
    }
}
