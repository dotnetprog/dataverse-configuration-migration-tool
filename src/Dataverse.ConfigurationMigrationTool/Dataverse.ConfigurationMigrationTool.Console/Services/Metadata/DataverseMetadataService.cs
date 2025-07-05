using Dataverse.ConfigurationMigrationTool.Console.Features.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.ServiceModel;

namespace Dataverse.ConfigurationMigrationTool.Console.Services.Metadata;

public class DataverseMetadataService : IMetadataService
{
    private readonly IOrganizationServiceAsync2 _organizationServiceAsync2;
    private readonly ILogger<DataverseMetadataService> _logger;

    public DataverseMetadataService(IOrganizationServiceAsync2 organizationServiceAsync2,
        ILogger<DataverseMetadataService> logger)
    {
        _organizationServiceAsync2 = organizationServiceAsync2;
        _logger = logger;
    }

    public async Task<EntityMetadata> GetEntity(string logicalName)
    {
        try
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = logicalName,
                EntityFilters = EntityFilters.All,
                RetrieveAsIfPublished = false
            };
            var response = await _organizationServiceAsync2.ExecuteAsync(request) as RetrieveEntityResponse;
            return response.EntityMetadata;
        }
        catch (FaultException<OrganizationServiceFault> faultEx)
        {
            _logger.LogTrace(faultEx, faultEx.Message);
            return null;
        }

    }

    public async Task<ManyToManyRelationshipMetadata> GetRelationShipM2M(string logicalName)
    {
        var entity = await GetEntity(logicalName);
        return entity?.ManyToManyRelationships?.FirstOrDefault();

    }
}
