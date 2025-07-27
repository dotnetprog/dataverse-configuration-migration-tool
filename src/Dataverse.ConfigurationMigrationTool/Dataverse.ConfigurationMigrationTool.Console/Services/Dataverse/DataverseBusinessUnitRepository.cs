using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public class DataverseBusinessUnitRepository : IBusinessUnitRepository
{
    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly IMemoryCache _memoryCache;

    public DataverseBusinessUnitRepository(IMemoryCache memoryCache, IOrganizationServiceAsync2 orgService)
    {
        _memoryCache = memoryCache;
        _orgService = orgService;
    }

    public async Task<Entity> GetByIdAsync(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"bu.GetByIdAsync.{Id}", async (_) =>
        {
            var query = new QueryExpression("businessunit")
            {
                ColumnSet = new ColumnSet("businessunitid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("businessunitid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetByNameAsync(string businessUnitName)
    {
        return await _memoryCache.GetOrCreateAsync($"bu.GetByNameAsync.{businessUnitName}", async (_) =>
        {
            var query = new QueryExpression("businessunit")
            {
                ColumnSet = new ColumnSet("businessunitid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Equal, businessUnitName)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
}
