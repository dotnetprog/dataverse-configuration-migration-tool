using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public class DataverseTeamRepository : ITeamRepository
{
    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly IMemoryCache _memoryCache;

    public DataverseTeamRepository(IOrganizationServiceAsync2 orgService, IMemoryCache memoryCache)
    {
        _orgService = orgService;
        _memoryCache = memoryCache;
    }
    public async Task<Entity> GetByIdAsync(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"team.GetByIdAsync.{Id}", async (_) =>
        {
            var query = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet("teamid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("teamid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
    public async Task<Entity> GetByNameAsync(string teamName)
    {

        return await _memoryCache.GetOrCreateAsync($"team.GetByNameAsync.{teamName}", async (_) =>
        {
            var query = new QueryExpression("team")
            {
                ColumnSet = new ColumnSet("teamid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Equal, teamName)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
}
