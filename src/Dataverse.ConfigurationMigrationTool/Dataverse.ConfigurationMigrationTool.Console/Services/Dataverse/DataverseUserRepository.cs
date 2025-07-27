using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public class DataverseUserRepository : ISystemUserRepository
{
    private readonly IOrganizationServiceAsync2 _orgService;

    public DataverseUserRepository(IOrganizationServiceAsync2 orgService, IMemoryCache memoryCache)
    {
        _orgService = orgService;
        _memoryCache = memoryCache;
    }

    private readonly IMemoryCache _memoryCache;
    public async Task<Entity> GetByIdAsync(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"user.GetByIdAsync.{Id}", async (_) =>
        {
            var query = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid", "fullname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("systemuserid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
    public async Task<Entity> GetByName(string fullname)
    {
        return await _memoryCache.GetOrCreateAsync($"user.GetByName.{fullname}", async (_) =>
        {
            var query = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid", "fullname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("fullname", ConditionOperator.Equal, fullname)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
}
