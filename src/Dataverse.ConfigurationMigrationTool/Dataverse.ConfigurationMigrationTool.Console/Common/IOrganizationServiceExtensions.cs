using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dataverse.ConfigurationMigrationTool.Console.Common;
public static class IOrganizationServiceExtensions
{
    public static async Task<EntityCollection> RetrieveAll(this IOrganizationServiceAsync2 service, QueryExpression query, int page = 5000, ILogger _logger = null, CancellationToken cancellationToken = default)
    {
        // The records to return
        List<Entity> entities = new();

        // Set the page
        query.PageInfo.PageNumber = 1;
        // Set the count
        query.PageInfo.Count = page;

        while (true)
        {
            _logger?.LogInformation("Retrieving page {page} with {count} records", query.PageInfo.PageNumber, query.PageInfo.Count);
            // Get the records
            var results = await service.RetrieveMultipleAsync(query, cancellationToken);

            entities.AddRange(results.Entities);
            _logger?.LogInformation("Retrieved {count} records from page {page}", results.Entities.Count, query.PageInfo.PageNumber);
            _logger?.LogInformation("Total Retrieved {count} records", entities.Count);
            if (!results.MoreRecords)
            {
                //Stop if there are no more records
                break;
            }
            // Set the PagingCookie with the PagingCookie from the previous query
            query.PageInfo.PagingCookie = results.PagingCookie;

            // Update the PageNumber
            query.PageInfo.PageNumber++;
        }

        return new EntityCollection(entities);
    }
}
