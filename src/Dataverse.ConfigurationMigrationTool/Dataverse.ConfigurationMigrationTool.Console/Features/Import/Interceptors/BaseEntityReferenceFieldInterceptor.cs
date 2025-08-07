using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public abstract class BaseEntityReferenceFieldInterceptor : BaseEntityInterceptor
{
    protected string EntityName { get; }
    protected BaseEntityReferenceFieldInterceptor(string entityName)
    {
        EntityName = entityName;
    }

    protected abstract Task<Entity> GetEntityByIdAsync(Guid Id);
    protected abstract Task<Entity> GetEntityByNameAsync(string Name);
    public override async Task<Entity> InterceptAsync(Entity entity)
    {
        var attributes = entity.Attributes.Where(kv => kv.Value is EntityReference ef && ef.LogicalName == EntityName).ToList();
        foreach (var attribute in attributes)
        {
            var reference = attribute.Value as EntityReference;
            var resolvedTeam = await GetEntityByIdAsync(reference.Id);
            if (resolvedTeam != null)
            {
                continue;

            }
            resolvedTeam = await GetEntityByNameAsync(reference.Name);
            if (resolvedTeam != null)
            {
                entity[attribute.Key] = new EntityReference(EntityName, resolvedTeam.Id);

            }
            else
            {
                entity.Attributes.Remove(attribute.Key);
            }
        }
        return await base.InterceptAsync(entity);
    }

}
