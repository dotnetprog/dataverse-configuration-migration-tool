using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Xrm.Sdk;

namespace Dataverse.ConfigurationMigrationTool.Console.Features.Import.Interceptors;
public class BusinessUnitInterceptor : BaseEntityInterceptor
{
    private readonly IBusinessUnitRepository _businessUnitRepository;
    public BusinessUnitInterceptor(IBusinessUnitRepository businessUnitRepository)
    {
        _businessUnitRepository = businessUnitRepository;
    }
    public override async Task<Entity> InterceptAsync(Entity entity)
    {
        var attributes = entity.Attributes.Where(kv => kv.Value is EntityReference ef && ef.LogicalName == "businessunit").ToList();
        foreach (var attribute in attributes)
        {
            var reference = attribute.Value as EntityReference;
            var resolvedBu = await _businessUnitRepository.GetByIdAsync(reference.Id);
            if (resolvedBu != null)
            {
                continue;

            }
            resolvedBu = await _businessUnitRepository.GetByNameAsync(reference.Name);
            if (resolvedBu != null)
            {
                entity[attribute.Key] = new EntityReference("businessunit", resolvedBu.Id);

            }
            else
            {
                entity.Attributes.Remove(attribute.Key);
            }
        }

        return await base.InterceptAsync(entity);
    }
}
