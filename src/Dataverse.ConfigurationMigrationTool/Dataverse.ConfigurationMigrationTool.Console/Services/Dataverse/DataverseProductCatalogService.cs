using Microsoft.Extensions.Caching.Memory;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Dataverse;
public class DataverseProductCatalogService : IProductCatalogService
{
    private readonly IOrganizationServiceAsync2 _orgService;
    private readonly IMemoryCache _memoryCache;

    public DataverseProductCatalogService(IOrganizationServiceAsync2 orgService, IMemoryCache memoryCache)
    {
        _orgService = orgService;
        _memoryCache = memoryCache;
    }
    public async Task<Entity> GetTransacationCurrencyById(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetTransacationCurrencyById.{Id}", async (_) =>
        {
            var query = new QueryExpression("transactioncurrency")
            {
                ColumnSet = new ColumnSet("transactioncurrencyid", "currencyname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("transactioncurrencyid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetTransacationCurrencyByName(string name)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetTransacationCurrencyByName.{name}", async (_) =>
        {
            var query = new QueryExpression("transactioncurrency")
            {
                ColumnSet = new ColumnSet("transactioncurrencyid", "currencyname"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("currencyname", ConditionOperator.Equal, name)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetUoMById(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetUoMById.{Id}", async (_) =>
        {
            var query = new QueryExpression("uom")
            {
                ColumnSet = new ColumnSet("uomid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("uomid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetUoMByName(string name)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetUoMByName.{name}", async (_) =>
        {
            var query = new QueryExpression("uom")
            {
                ColumnSet = new ColumnSet("uomid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Equal, name)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetUoMScheduleById(Guid Id)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetUoMScheduleById.{Id}", async (_) =>
        {
            var query = new QueryExpression("uomschedule")
            {
                ColumnSet = new ColumnSet("uomscheduleid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                    new ConditionExpression("uomscheduleid", ConditionOperator.Equal, Id)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }

    public async Task<Entity> GetUoMScheduleByName(string name)
    {
        return await _memoryCache.GetOrCreateAsync($"product.GetUoMScheduleByName.{name}", async (_) =>
        {
            var query = new QueryExpression("uomschedule")
            {
                ColumnSet = new ColumnSet("uomscheduleid", "name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Equal, name)
                }
                }
            };
            var result = await _orgService.RetrieveMultipleAsync(query);
            return result.Entities.FirstOrDefault();
        });
    }
}
